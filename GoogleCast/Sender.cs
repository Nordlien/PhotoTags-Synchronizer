﻿using GoogleCast.Channels;
using GoogleCast.Messages;
using GoogleCast.Models.Receiver;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCast
{
    /// <summary>
    /// GoogleCast sender
    /// </summary>
    public class Sender : ISender
    {
        private const int RECEIVE_TIMEOUT = 30000;

        /// <summary>
        /// Raised when the sender is disconnected
        /// </summary>
        public event EventHandler Disconnected;

        /// <summary>
        /// Initializes a new instance of <see cref="Sender"/> class
        /// </summary>
        public Sender() : this(new ServiceCollection().AddGoogleCast().BuildServiceProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Sender"/> class
        /// </summary>
        /// <param name="serviceProvider">collection of service descriptors</param>
        public Sender(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            var channels = serviceProvider.GetServices<IChannel>();
            Channels = channels;
            foreach (var channel in channels)
            {
                channel.Sender = this;
            }
        }

        private IServiceProvider ServiceProvider { get; }
        private IEnumerable<IChannel> Channels { get; set; }
        private IReceiver Receiver { get; set; }
        private Stream NetworkStream { get; set; }
        private TcpClient TcpClient { get; set; }
        private SemaphoreSlim SendSemaphoreSlim { get; } = new SemaphoreSlim(1, 1);
        private SemaphoreSlim EnsureConnectionSemaphoreSlim { get; } = new SemaphoreSlim(1, 1);
        private ConcurrentDictionary<int, object> WaitingTasks { get; } = new ConcurrentDictionary<int, object>();
        private CancellationTokenSource CancellationTokenSource { get; set; }

        /// <summary>
        /// Disconnects
        /// </summary>
        public void Disconnect()
        {
            foreach (var channel in GetStatusChannels())
            {
                channel.Status = null;
            }
            Dispose();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Dispose()
        {
            if (TcpClient != null)
            {
                WaitingTasks.Clear();
                if (CancellationTokenSource != null)
                {
                    CancellationTokenSource.Cancel();
                    CancellationTokenSource = null;
                }
                Dispose(NetworkStream, () => NetworkStream = null);
                Dispose(TcpClient, () => TcpClient = null);
                OnDisconnected();
            }
        }

        private void Dispose(IDisposable disposable, Action action)
        {
            if (disposable != null)
            {
                try
                {
                    disposable.Dispose();
                }
                catch (Exception) { }
                finally
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Raises the Disconnected event
        /// </summary>
        protected virtual void OnDisconnected()
        {
            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets a channel
        /// </summary>
        /// <typeparam name="TChannel">channel type</typeparam>
        /// <returns>a channel</returns>
        public TChannel GetChannel<TChannel>() where TChannel : IChannel
        {
            return Channels.OfType<TChannel>().FirstOrDefault();
        }

        /// <summary>
        /// Connects to a receiver
        /// </summary>
        /// <param name="receiver">receiver</param>
        public async Task ConnectAsync(IReceiver receiver)
        {
            Dispose();

            Receiver = receiver;
            var tcpClient = new TcpClient();
            TcpClient = tcpClient;
            var ipEndPoint = receiver.IPEndPoint;
            var host = ipEndPoint.Address.ToString();
            await tcpClient.ConnectAsync(host, ipEndPoint.Port);
            var secureStream = new SslStream(tcpClient.GetStream(), true, (sender, certificate, chain, sslPolicyErrors) => true);
            await secureStream.AuthenticateAsClientAsync(host);
            NetworkStream = secureStream;

            Receive();
            await GetChannel<IConnectionChannel>().ConnectAsync();
        }

        private void Receive()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            CancellationTokenSource = cancellationTokenSource;
            Task.Run(async () =>
            {
                try
                {
                    var channels = Channels;
                    var messageTypes = ServiceProvider.GetService<IMessageTypes>();
                    while (true)
                    {
                        var buffer = await ReadAsync(4, cancellationToken);
                        if (BitConverter.IsLittleEndian)
                        {
                            Array.Reverse(buffer);
                        }
                        var length = BitConverter.ToInt32(buffer, 0);
                        CastMessage castMessage;
                        using (var ms = new MemoryStream())
                        {
                            await ms.WriteAsync(await ReadAsync(length, cancellationToken), 0, length, cancellationToken);
                            ms.Position = 0;
                            castMessage = Serializer.Deserialize<CastMessage>(ms);
                        }
                        var payload = (castMessage.PayloadType == PayloadType.Binary ?
                            Encoding.UTF8.GetString(castMessage.PayloadBinary) : castMessage.PayloadUtf8);
                        
                        var channel = channels.FirstOrDefault(c => c.Namespace == castMessage.Namespace);
                        if (channel != null)
                        {
                            var message = JsonSerializer.Deserialize<MessageWithId>(payload);
                            if (messageTypes.TryGetValue(message.Type, out Type type))
                            {
                                try
                                {
                                    var response = (IMessage)JsonSerializer.Deserialize(type, payload);
                                    await channel.OnMessageReceivedAsync(response);
                                    TaskCompletionSourceInvoke(message, "SetResult", response);
                                }
                                catch (Exception ex)
                                {
                                    TaskCompletionSourceInvoke(message, "SetException", ex, new Type[] { typeof(Exception) });
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    CancellationTokenSource = null;
                    Dispose();
                }
            });
        }

        private void TaskCompletionSourceInvoke(MessageWithId message, string method, object parameter, Type[] types = null)
        {
            if (message.HasRequestId && WaitingTasks.TryRemove(message.RequestId, out object tcs))
            {
                var tcsType = tcs.GetType();
                (types == null ? tcsType.GetMethod(method) : tcsType.GetMethod(method, types)).Invoke(tcs, new object[] { parameter });
            }
        }

        private async Task<byte[]> ReadAsync(int bufferLength, CancellationToken cancellationToken)
        {
            var buffer = new byte[bufferLength];
            int nb, length = 0;
            while (length < bufferLength)
            {
                if (NetworkStream != null) nb = await NetworkStream.ReadAsync(buffer, length, bufferLength - length, cancellationToken);
                else nb = 0;
                if (nb == 0)
                {
                    throw new InvalidOperationException();
                }
                length += nb;
            }
            return buffer;
        }

        private async Task EnsureConnection()
        {
            if (TcpClient == null && Receiver != null)
            {
                await EnsureConnectionSemaphoreSlim.WaitAsync();
                try
                {
                    if (TcpClient == null && Receiver != null)
                    {
                        await ConnectAsync(Receiver);
                    }
                }
                finally
                {
                    EnsureConnectionSemaphoreSlim.Release();
                }
            }
        }

        private async Task SendAsync(CastMessage castMessage)
        {
            await EnsureConnection();

            await SendSemaphoreSlim.WaitAsync();
            try
            {
                byte[] message;
                using (var ms = new MemoryStream())
                {
                    Serializer.Serialize(ms, castMessage);
                    message = ms.ToArray();
                }
                var header = BitConverter.GetBytes(message.Length);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(header);
                }
                var networkStream = NetworkStream;
                await networkStream.WriteAsync(header, 0, header.Length);
                await networkStream.WriteAsync(message, 0, message.Length);
                await networkStream.FlushAsync();
            }
            finally
            {
                SendSemaphoreSlim.Release();
            }
        }

        private CastMessage CreateCastMessage(string ns, string destinationId)
        {
            return new CastMessage()
            {
                Namespace = ns,
                SourceId = DefaultIdentifiers.SENDER_ID,
                DestinationId = destinationId
            };
        }

        /// <summary>
        /// Launches an application
        /// </summary>
        /// <typeparam name="TAppChannel">application channel type</typeparam>
        /// <returns>receiver status</returns>
        public async Task<ReceiverStatus> LaunchAsync<TAppChannel>() where TAppChannel : IApplicationChannel
        {
            return await LaunchAsync(GetChannel<TAppChannel>());
        }

        /// <summary>
        /// Launches an application
        /// </summary>
        /// <param name="applicationChannel">application channel</param>
        /// <returns>receiver status</returns>
        public async Task<ReceiverStatus> LaunchAsync(IApplicationChannel applicationChannel)
        {
            return await LaunchAsync(applicationChannel.ApplicationId);
        }

        /// <summary>
        /// Launches an application
        /// </summary>
        /// <param name="applicationId">application identifier</param>
        /// <returns>receiver status</returns>
        public async Task<ReceiverStatus> LaunchAsync(string applicationId)
        {
            return await GetChannel<IReceiverChannel>().LaunchAsync(applicationId);
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="ns">namespace</param>
        /// <param name="message">message to send</param>
        /// <param name="destinationId">destination identifier</param>
        public async Task SendAsync(string ns, IMessage message, string destinationId)
        {
            var castMessage = CreateCastMessage(ns, destinationId);
            castMessage.PayloadUtf8 = JsonSerializer.SerializeToUTF8String(message);
            await SendAsync(castMessage);
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <typeparam name="TResponse">response type</typeparam>
        /// <param name="ns">namespace</param>
        /// <param name="message">message to send</param>
        /// <param name="destinationId">destination identifier</param>
        /// <returns>the result</returns>
        public async Task<TResponse> SendAsync<TResponse>(string ns, IMessageWithId message, string destinationId)
            where TResponse : IMessageWithId
        {
            var taskCompletionSource = new TaskCompletionSource<TResponse>();
            WaitingTasks[message.RequestId] = taskCompletionSource;
            await SendAsync(ns, message, destinationId);
            return await taskCompletionSource.Task.TimeoutAfter(RECEIVE_TIMEOUT);
        }

        private IEnumerable<IStatusChannel> GetStatusChannels()
        {
            return Channels.OfType<IStatusChannel>();
        }

        /// <summary>
        /// Gets the differents statuses
        /// </summary>
        /// <returns>a dictionnary of namespace/status</returns>
        public IDictionary<string, object> GetStatuses()
        {
            return GetStatusChannels().ToDictionary(c => c.Namespace, c => c.Status);
        }

        /// <summary>
        /// Restore the differents statuses
        /// </summary>
        /// <param name="statuses">statuses to restore</param>
        public void RestoreStatuses(IDictionary<string, object> statuses)
        {
            var channels = GetStatusChannels();
            IStatusChannel channel;
            foreach (var keyValuePair in statuses)
            {
                channel = channels.FirstOrDefault(c => c.Namespace == keyValuePair.Key);
                if (channel != null)
                {
                    channel.Status = keyValuePair.Value;
                }
            }
        }
    }
}
