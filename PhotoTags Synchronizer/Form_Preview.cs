using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using LibVLCSharp.Shared;
using System.Linq;
using System.Diagnostics;
using NHttp;
using ImageAndMovieFileExtentions;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using GoogleCast.Models.Media;

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : Form
    {
        
        #region Preview - globale variables
        private List<string> previewItems = new List<string>();
        private int previewMediaindex = 0;
        private bool canPlayAndPause = false;

        private float vlcVolume = 1;
        private float chromcastVolume = 1;
        private long vlcTime = 0;
        private float vlcPosition = 0;
        Stopwatch stopwachVlcMediaPositionChanged = new Stopwatch();
        #endregion

        #region Preview Media -- Click  ---
        private void mediaPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripDropDownButtonChromecastList.Enabled = false;

            buttonStateVlc = ButtonStateVlcChromcastState.Disconnected;
            buttonStateChromecast = ButtonStateVlcChromcastState.Disconnected;
            isChromecasting = false;
            isChromecastStopClicked = false;
            isChromecastCloseClicked = false;

            GoogleCastInitSender();
            GoogleCastFindReceiversAsync();

            PreviewInitVlc();
            PreviewInitControls();
            PreviewSlideshowWait();
        }
        #endregion

        #region Preview - Init - Vlc
        private void PreviewInitVlc()
        {
            videoView1.MediaPlayer.EnableKeyInput = true;
            videoView1.MediaPlayer.EnableHardwareDecoding = true;
            videoView1.MediaPlayer.EnableKeyInput = true;

            vlcMediaPlayerVideoView.Backward += VlcMediaPlayerVideoView_Backward;
            vlcMediaPlayerVideoView.Forward += VlcMediaPlayerVideoView_Forward; ;
            vlcMediaPlayerVideoView.Buffering += VlcMediaPlayerVideoView_Buffering;
            vlcMediaPlayerVideoView.EncounteredError += VlcMediaPlayerVideoView_EncounteredError;

            vlcMediaPlayerVideoView.Muted += VlcMediaPlayerVideoView_Muted;
            vlcMediaPlayerVideoView.Opening += VlcMediaPlayerVideoView_Opening;

            vlcMediaPlayerVideoView.Paused += VlcMediaPlayerVideoView_Paused;
            vlcMediaPlayerVideoView.PositionChanged += VlcMediaPlayerVideoView_PositionChanged;

            vlcMediaPlayerVideoView.EndReached += VlcMediaPlayerVideoView_EndReached;
            vlcMediaPlayerVideoView.Playing += VlcMediaPlayerVideoView_Playing;

            vlcMediaPlayerVideoView.Stopped += VlcMediaPlayerVideoView_Stopped;

            vlcMediaPlayerVideoView.TimeChanged += VlcMediaPlayerVideoView_TimeChanged;
            vlcMediaPlayerVideoView.Unmuted += VlcMediaPlayerVideoView_Unmuted;
            vlcMediaPlayerVideoView.VolumeChanged += VlcMediaPlayerVideoView_VolumeChanged;
        }
        #endregion

        #region Preview - Init - Controls
        private void PreviewInitControls()
        {
            SlideShowInit();


            stopwachVlcMediaPositionChanged.Restart();
            stopwachMediaTimeChanged.Restart();

            toolStripTraceBarItemMediaPreviewTimer.TrackBar.Maximum = 0;
            toolStripTraceBarItemMediaPreviewTimer.TrackBar.Maximum = 100;
            toolStripTraceBarItemMediaPreviewTimer.TrackBar.Height = 26;
            toolStripTraceBarItemMediaPreviewTimer.TrackBar.Width = 204;

            panelMediaPreview.Dock = DockStyle.Fill;
            panelMediaPreview.Visible = true;

            imageBoxPreview.Visible = false;
            imageBoxPreview.Dock = DockStyle.Fill;

            videoView1.Visible = false;
            videoView1.Dock = DockStyle.Fill;

            previewItems.Clear();
            toolStripDropDownButtonMediaList.DropDownItems.Clear();
            for (int selectedItemIndex = 0; selectedItemIndex < imageListView1.SelectedItems.Count; selectedItemIndex++)
            {
                previewItems.Add(imageListView1.SelectedItems[selectedItemIndex].FileFullPath);

                ToolStripMenuItem toolStripDropDownItem = new ToolStripMenuItem();
                toolStripDropDownItem.Click += ToolStripDropDownItemPreviewMedia_Click;
                toolStripDropDownItem.Text = imageListView1.SelectedItems[selectedItemIndex].FileFullPath;
                toolStripDropDownItem.Tag = selectedItemIndex;
                toolStripDropDownButtonMediaList.DropDownItems.Add(toolStripDropDownItem);
            }

            if (previewItems.Count > 0)
            {
                previewMediaindex = 0;
                ShowPreviewItem(previewItems[previewMediaindex]);
            }
        }
        #endregion 

        #region Preview - GoogleCast - Init sender vaiables
        private void GoogleCastInitSender()
        {
            if (googleCast_sender == null && !isChromecastStopClicked)
            {
                isChromecastStopClicked = false;

                googleCast_sender = new GoogleCast.Sender();
                googleCast_sender.Disconnected += GoogleCast_sender_Disconnected;
                googleCast_sender.GetChannel<GoogleCast.Channels.IMediaChannel>().StatusChanged += GoogleCast_mediaChannel_StatusChanged;
                googleCast_sender.GetChannel<GoogleCast.Channels.IReceiverChannel>().StatusChanged += GoogleCast_ReceiverChannel_StatusChanged;
                SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Init, VlcChromecast.Chromecast);
            }
        }
        #endregion 

        #region Preview - GoogleCast - FindReceiversAsync
        private async void GoogleCastFindReceiversAsync()
        {
            //if (isChromecastStopClicked) return;
            timerFindGoogleCast.Stop();

            // Use the DeviceLocator to find a Chromecast
            googleCast_DeviceLocator = new GoogleCast.DeviceLocator();

            googleCast_receivers = await googleCast_DeviceLocator.FindReceiversAsync();

            toolStripDropDownButtonChromecastList.Enabled = false;
            toolStripDropDownButtonChromecastList.DropDownItems.Clear();
            foreach (GoogleCast.IReceiver googleCast_receiver in googleCast_receivers)
            {
                ToolStripMenuItem toolStripDropDownItem = new ToolStripMenuItem();
                toolStripDropDownItem.Click += ToolStripDropDownItemPreviewChromecast_Click;
                toolStripDropDownItem.Text = googleCast_receiver.FriendlyName;
                toolStripDropDownItem.Tag = googleCast_receiver;
                toolStripDropDownButtonChromecastList.DropDownItems.Add(toolStripDropDownItem);
            }

            if (toolStripDropDownButtonChromecastList.DropDownItems.Count > 0) toolStripDropDownButtonChromecastList.Enabled = true;
            timerFindGoogleCast.Start();
        }

        private void timerFindGoogleCast_Tick(object sender, EventArgs e)
        {
            GoogleCastFindReceiversAsync();
        }
        #endregion

        #region GoogleCast - Internal Commands
        //https://github.com/kakone/GoogleCast
        private GoogleCast.DeviceLocator googleCast_DeviceLocator = null;
        private IEnumerable<GoogleCast.IReceiver> googleCast_receivers = null;
        private bool googleCast_IsReceiverConnected = false;
        private GoogleCast.IReceiver googleCast_SelectedReceiver = null;
        private GoogleCast.Sender googleCast_sender;
        private bool isChromecastStopClicked = false;
        private bool isChromecastCloseClicked = false;

        private string mediaPlaying = "";

        private string mimeFormatImage = "image/jpeg";
        private string mimeFormatVideo = "video/mp4";

        #region GoogleCast - IsMediaChannelConnected
        private bool IsMediaChannelConnected()
        {
            if (googleCast_sender == null) return false;
            GoogleCast.Channels.IReceiverChannel receiverChannel = googleCast_sender?.GetChannel<GoogleCast.Channels.IReceiverChannel>();
            return receiverChannel != null && receiverChannel?.Status != null; // && receiverChannel?.Status.Applications != null;
        }
        #endregion 

        #region GoogleCast - IsApplicationStarted
        private bool IsApplicationStarted()
        {
            if (googleCast_sender == null) return false;
            GoogleCast.Channels.IReceiverChannel receiverChannel = googleCast_sender?.GetChannel<GoogleCast.Channels.IReceiverChannel>();
            return receiverChannel != null && receiverChannel?.Status != null && receiverChannel?.Status.Applications != null;
        }
        #endregion

        #region GoogleCast - IsMediaChannelStopped
        private bool googleCast_IsMediaChannelStopped
        {
            get
            {
                if (googleCast_sender == null) return true;
                GoogleCast.Channels.IMediaChannel mediaChannel = googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>();
                return (googleCast_sender == null || mediaChannel?.Status == null || !string.IsNullOrEmpty(mediaChannel?.Status?.FirstOrDefault()?.IdleReason));
            }
        }
        #endregion

        #region GoogleCast - ConnectReceiver 
        private void ConnectReceiver()
        {
            _ = ConnectReceiverAsync();
        }
        #endregion 

        #region GoogleCast - ConnectReceiver - Async
        private async Task<bool> ConnectReceiverAsync()
        {
            if (!googleCast_IsReceiverConnected && googleCast_sender != null && googleCast_SelectedReceiver != null)
            {
                try
                {
                    await googleCast_sender.ConnectAsync(googleCast_SelectedReceiver);
                    googleCast_IsReceiverConnected = true;
                    return true;
                }
                catch
                {
                    googleCast_IsReceiverConnected = false;
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region GoogleCast - ConnectMediaChannel
        private void GoogleCastConnect()
        {
            _ = ConnectMediaChannelAsync();
        }
        #endregion

        #region GoogleCast - ConnectMediaChannel- Async
        private async Task<bool> ConnectMediaChannelAsync()
        {
            if (googleCast_sender == null) return false;

            if (!IsMediaChannelConnected() || googleCast_IsMediaChannelStopped)
            {
                if (await ConnectReceiverAsync())
                {
                    try
                    {
                        var mediaChannel = googleCast_sender.GetChannel<GoogleCast.Channels.IMediaChannel>();
                        if (!IsMediaChannelConnected())
                            await googleCast_sender.LaunchAsync(googleCast_sender.GetChannel<GoogleCast.Channels.IMediaChannel>());
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Communication with Chromecast failed... \r\n" + ex.Message);
                        return false;
                    }

                }
                else
                {
                    MessageBox.Show("Communication with Chromecast failed to reconnect...");
                    return false;
                }
            }

            return true;
        }
        #endregion 

        #region GoogleCast - Play / Load media
        private async void GoogleCast_Play(string contentSource, string fileExtention)
        {
            if (await ConnectMediaChannelAsync() && IsApplicationStarted())
            {
                if (mediaPlaying != contentSource)
                {
                    GoogleCast.Models.Media.MediaStatus googleCast_CurrentMediaStatus = null;

                    string contentType = "";
                    if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fileExtention)) contentType = mimeFormatImage;
                    if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fileExtention)) contentType = mimeFormatVideo;

                    try
                    {
                        mediaPlaying = contentSource;
                        googleCast_CurrentMediaStatus = await googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>().LoadAsync(new MediaInformation() { ContentType = contentType, ContentId = contentSource });
                        if (googleCast_CurrentMediaStatus != null) mediaPlaying = "";
                        else MessageBox.Show("Chromecast failed to load media:\r\n" + contentSource);
                    }
                    catch (Exception ex)
                    {
                        mediaPlaying = "";
                        MessageBox.Show("Communication with Chormecast failed... \r\n" + contentSource + "\r\nError message:" + ex.Message);
                    }

                }
                else
                {
                    try
                    {
                        mediaPlaying = contentSource;
                        await googleCast_sender.GetChannel<GoogleCast.Channels.IMediaChannel>().PlayAsync();
                    }
                    catch (Exception ex)
                    {
                        mediaPlaying = "";
                        MessageBox.Show("Communication with Google Cast failed continue play... \r\n" + ex.Message);
                    }
                }

            }
            else
            {
                MessageBox.Show("Communication with Chromecast failed to reconnect...");
                //googleCast_IsMediaChannelConnected = false;
            }
        }
        #endregion

        #region GoogleCast - Play (Resume Play)
        private async void GoogleCast_Play()
        {
            try
            {
                if (!googleCast_IsMediaChannelStopped) await googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>().PlayAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Communication with Google Cast failed to play... \r\n" + ex.Message);
            }
        }
        #endregion 

        #region GoogleCast - Pause
        private async void GoogleCast_Pause()
        {
            try
            {
                if (!googleCast_IsMediaChannelStopped) await googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>().PauseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Communication with Google Cast failed to pause... \r\n" + ex.Message);
            }
        }
        #endregion 

        #region GoogleCast - Stop / Disconnect
        private void GoogleCast_Stop(bool disconnect)
        {
            isChromecastStopClicked = true;
            SelectedDevice(null);
        }
        #endregion

        #region GoogleCast - Disconnect *** NOT IN USED ****
        private void GoogleCastDisconnect()
        {


            if (googleCast_sender != null)
            {
                try
                {
                    googleCast_sender.Disconnect();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Diconnected failed...\r\n" + ex.Message);
                }
            }
        }
        #endregion

        #region GoogleCast - Select Receiver
        private async void SelectedDevice(GoogleCast.IReceiver selectedNewReceiver)
        {
            if (googleCast_SelectedReceiver == selectedNewReceiver) return;

            if (googleCast_IsReceiverConnected)
            {
                if (!googleCast_IsMediaChannelStopped)
                {
                    await googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>().StopAsync();
                    mediaPlaying = "";
                    Thread.Sleep(600);
                }

                if (googleCast_IsMediaChannelStopped) //Implemented
                {

                    if (IsMediaChannelConnected() || await ConnectReceiverAsync()) //Implemented
                    {
                        try
                        {
                            googleCast_sender.GetChannel<GoogleCast.Channels.IMediaChannel>().StatusChanged -= GoogleCast_mediaChannel_StatusChanged;
                            googleCast_sender.GetChannel<GoogleCast.Channels.IReceiverChannel>().StatusChanged -= GoogleCast_ReceiverChannel_StatusChanged;
                            await googleCast_sender.GetChannel<GoogleCast.Channels.IReceiverChannel>().StopAsync();
                        }
                        finally
                        {
                            Thread.Sleep(400);
                            mediaPlaying = "";
                            googleCast_sender = null;
                            googleCast_IsReceiverConnected = false;
                        }
                    }
                }
            }
            //googleCast_IsMediaChannelConnected = false;

            googleCast_IsReceiverConnected = false;
            googleCast_SelectedReceiver = selectedNewReceiver;
            if (selectedNewReceiver != null) ConnectReceiver();
        }

        #endregion

        #endregion

        #region GoogleCast - Events

        #region GoogleCast - Event - PlayerState Changed - Information
        private string MediaStatusToText(MediaStatus status, int ext = 0)
        {
            if (status == null) return "";
            return
                (ext == 0 ? "" : "Extended Status Level: " + ext.ToString() + "\r\n") +
                "CurrentItemId:   " + status.CurrentItemId.ToString() + "\r\n" +
                "CurrentTime:     " + status.CurrentTime.ToString() + "\r\n" +
                "IdleReason:      " + (status.IdleReason == null ? "null" : status.IdleReason) + "\r\n" +
                "PlaybackRate:    " + status.PlaybackRate.ToString() + "\r\n" +
                "PlayerState:     " + (status.PlayerState == null ? "null" : status.PlayerState) + "\r\n" +
                "RepeatMode:      " + (status.RepeatMode == null ? "null" : status.RepeatMode) + "\r\n" +
                "SupportedMedia:  " + status.SupportedMediaCommands.ToString() + "\r\n" +
                MediaStatusToText(status.ExtendedStatus, ext + 1);

            //"Ext.PlayerState: " + (status.ExtendedStatus?.PlayerState == null ? "null" : status.ExtendedStatus?.PlayerState) + "\r\n" +
            //"Ext.IdleReason:  " + (status.ExtendedStatus?.IdleReason == null ? "null" : status.ExtendedStatus?.IdleReason) + "\r\n";
        }
        #endregion

        #region GoogleCast - Event - Convert Status

        private void GoogleCast_StatusChanged(MediaStatus status, int ext = 0)
        {
            if (status != null)
            {
                switch (status?.PlayerState)
                {
                    case "IDLE":
                        string idleReason = String.IsNullOrEmpty(status.IdleReason) ? "" : status.IdleReason;
                        switch (idleReason)
                        {
                            case "CANCELLED":
                                SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Cancelled, VlcChromecast.Chromecast);
                                break;
                            case "INTERRUPTED":
                                SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Interrupted, VlcChromecast.Chromecast);
                                break;
                            case "FINISHED":
                                SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.EndReached, VlcChromecast.Chromecast);
                                break;
                            default:
                                if (status.ExtendedStatus == null)
                                {
                                    MessageBox.Show(MediaStatusToText(status));
                                    Console.WriteLine("GoogleCast_mediaChannel_StatusChanged: " + MediaStatusToText(status, ext));
                                }
                                break;
                        }
                        break;
                    case "BUFFERING":
                        SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Buffering, VlcChromecast.Chromecast);
                        break;
                    case "LOADING":
                        SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Opening, VlcChromecast.Chromecast);
                        break;
                    case "PLAYING":
                        SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Playing, VlcChromecast.Chromecast);
                        break;
                    case "PAUSED":
                        SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Paused, VlcChromecast.Chromecast);
                        break;
                    default:
                        MessageBox.Show(MediaStatusToText(status));
                        Console.WriteLine("GoogleCast_mediaChannel_StatusChanged: " + MediaStatusToText(status, ext));
                        break;
                }
                if (status.ExtendedStatus != null) GoogleCast_StatusChanged(status.ExtendedStatus, ext + 1);
                if (status != null)
                {
                    if (lastKnownChromeCastCurrentTime != status.CurrentTime)
                    {
                        lastKnownChromeCastCurrentTime = status.CurrentTime;
                        SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.TimeChanged, VlcChromecast.Chromecast);
                    }
                }
            }
        }
        #endregion

        #region GoogleCast - Event - Connecting + Volume Changed 
        private void GoogleCast_ReceiverChannel_StatusChanged(object sender, EventArgs e)
        {
            var status = ((GoogleCast.Channels.IReceiverChannel)sender)?.Status;

            if (status != null)
            {
                if (status.Applications == null)
                    SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Connecting, VlcChromecast.Chromecast);
                else
                    SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Connected, VlcChromecast.Chromecast);

                if (status.Volume.Level != null)
                {
                    chromcastVolume = (float)status.Volume.Level;
                    SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.VolumeChanged, VlcChromecast.Chromecast);
                }
                if (status.Volume.IsMuted != null)
                {
                    if ((bool)status.Volume.IsMuted) SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Mute, VlcChromecast.Chromecast);
                    else SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Unmute, VlcChromecast.Chromecast);
                }
            }
        }
        #endregion

        #region GoogleCast - Event - Disconnected 
        private void GoogleCast_sender_Disconnected(object sender, EventArgs e)
        {
            mediaPlaying = "";
            googleCast_IsReceiverConnected = false;
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Disconnected, VlcChromecast.Chromecast);
        }
        #endregion 

        #region GoogleCast - Event - Status Changed
        private void GoogleCast_mediaChannel_StatusChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(GoogleCast_mediaChannel_StatusChanged), sender, e);
                return;
            }

            var status = ((GoogleCast.Channels.IMediaChannel)sender)?.Status?.FirstOrDefault();
            GoogleCast_StatusChanged(status);
        }
        #endregion

        #endregion 

        #region GoogleCast - Actions

        #region GoogleCast - Chromecast_Click
        private void ToolStripDropDownItemPreviewChromecast_Click(object sender, EventArgs e)
        {
            isChromecastStopClicked = false;
            GoogleCastInitSender();

            // abort casting if no renderer items were found
            if (googleCast_receivers == null || googleCast_receivers.ToList<GoogleCast.IReceiver>().Count == 0)
            {
                MessageBox.Show("No renderer items found. Abort casting...");
                return;
            }

            foreach (ToolStripMenuItem toolStripDropDownItem in toolStripDropDownButtonChromecastList.DropDownItems) toolStripDropDownItem.Checked = false;

            ToolStripMenuItem clickedToolStripMenuItem = (ToolStripMenuItem)sender;
            clickedToolStripMenuItem.Checked = true;

            if (previewItems.Count == 0) return;
            googleCast_SelectedReceiver = (GoogleCast.IReceiver)clickedToolStripMenuItem.Tag;

            ShowMediaChromecast();
        }
        #endregion

        #region ShowMediaShromcast
        private void ShowMediaChromecast()
        {
            if (googleCast_SelectedReceiver == null) return;
            if (isChromecastStopClicked) return;
            if (isChromecastCloseClicked) return;

            SelectedDevice(googleCast_SelectedReceiver);

            string playItem =
                String.Format("http://{0}", nHttpServer.EndPoint) + "/chromecast?index=" + previewMediaindex + "&loadmedia=" +
                System.Web.HttpUtility.UrlEncode(previewItems[previewMediaindex]);

            GoogleCast_Play(playItem, previewItems[previewMediaindex]);
        }
        #endregion

        #endregion

        #region nHttpServer
        HttpServer nHttpServer = null;
        private Thread _ThreadHttp = null;
        private AutoResetEvent WaitApplicationClosing = null;

        #region nHttpServer - GetLocalIp 
        public string GetLocalIp()
        {
            string localIP = null;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }
        #endregion 

        #region nHttpServer - GetOpenPort
        private int GetOpenPort()
        {
            int PortStartIndex = 51000 + (new Random()).Next(0, 1000);
            int PortEndIndex = PortStartIndex + 1000;
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();

            List<int> usedPorts = tcpEndPoints.Select(p => p.Port).ToList<int>();
            int unusedPort = 0;

            for (int port = PortStartIndex; port < PortEndIndex; port++)
            {
                if (!usedPorts.Contains(port))
                {
                    unusedPort = port;
                    break;
                }
            }
            return unusedPort;
        }
        #endregion 

        #region nHttpServer - UnhandledException
        private void NHttpServer_UnhandledException(object sender, HttpExceptionEventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, HttpExceptionEventArgs>(NHttpServer_UnhandledException), sender, e);
                return;
            }
            toolStripLabelMediaPreviewStatus.Text = "nHTTP server unhandled exception...";
            Logger.Error(e.Request.ToString());
        }
        #endregion 

        #region nHttpServer - StateChanged
        private void NHttpServer_StateChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<object, EventArgs>(NHttpServer_StateChanged), sender, e);
                return;
            }
            if (nHttpServer != null) toolStripLabelMediaPreviewStatus.Text = "nHTTP server new state: " + nHttpServer.State.ToString();
            Console.WriteLine("nHTTP server new state: " + nHttpServer.State.ToString());
        }
        #endregion 

        #region nHttpServer - RequestReceived
        private void NHttpServer_RequestReceived(object sender, HttpRequestEventArgs e)
        {
            byte[] mediaByteArray = null;
            string mediaFullFilename = e.Request.Params["loadmedia"];
            string indexString = e.Request.Params["index"];
            if (int.TryParse(indexString, out int indexMediaFile))
            {
                if (indexMediaFile > -1 && indexMediaFile < previewItems.Count) mediaFullFilename = previewItems[indexMediaFile];
            }

            //if (e.Request.Path.ToLower() == "/favicon.ico") bilde = File.ReadAllBytes("favicon.png");
            if (e.Request.Path.ToLower() == "/chromecast" && mediaFullFilename != null)
            {
                if (ImageAndMovieFileExtentionsUtility.IsImageFormat(mediaFullFilename))
                {

                    mediaByteArray = ImageAndMovieFileExtentionsUtility.LoadAndConvertImage(mediaFullFilename, mimeFormatImage, 720, 480);
                    e.Response.CacheControl = "";
                    e.Response.ContentType = mimeFormatImage;
                }
                else if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(mediaFullFilename))
                {
                    mediaByteArray = File.ReadAllBytes(mediaFullFilename);
                    e.Response.ContentType = mimeFormatVideo;
                }
            }

            //e.Response.ContentType = "";
            e.Response.ExpiresAbsolute = DateTime.Now.AddDays(1);
            e.Response.CharSet = "";

            if (mediaByteArray != null) e.Response.OutputStream.Write(mediaByteArray, 0, mediaByteArray.Length);
            else e.Response.Status = "404 Not Found";

        }
        #endregion

        #endregion


        #region ButtonStates based on events 
        private ButtonStateVlcChromcastState buttonStateVlc = ButtonStateVlcChromcastState.Disconnected;
        private ButtonStateVlcChromcastState buttonStateChromecast = ButtonStateVlcChromcastState.Disconnected;
        private bool isChromecasting = false;
        private bool isPlayingVideoEndReached = false;
        private double lastKnownChromeCastCurrentTime = 0;

        #region ButtonStates based on events - ButtonStateVlcChromcastState
        enum ButtonStateVlcChromcastState
        {
            Init,
            Opening,
            Playing,
            Paused,
            Stopped,
            Cancelled,
            Interrupted,
            EndReached,

            Connecting,
            Connected,
            Disconnected,
            Buffering,
            Backward,
            Forward,
            VolumeChanged,
            Mute,
            Unmute,
            TimeChanged,
            PositionChanged,
            Error
        }
        #endregion

        #region ButtonStates based on events - VlcChromecast
        enum VlcChromecast
        {
            Vlc,
            Chromecast,
            Image
        }
        #endregion

        #region ButtonStates based on events - ConvertMsToHuman
        private string ConvertMsToHuman(long ms)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(ms);
            return string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
        }
        #endregion

        #region ButtonStates based on events
        private void SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState buttonStateVlcChromcastState, VlcChromecast vlcChromecast)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ButtonStateVlcChromcastState, VlcChromecast>(SetButtonStateVlcChromcastInit), buttonStateVlcChromcastState, vlcChromecast);
                return;
            }


            ButtonStateVlcChromcastState buttonPreviousStateVlc = buttonStateVlc;
            ButtonStateVlcChromcastState buttonPreviousStateChromecast = buttonStateChromecast;

            switch (vlcChromecast)
            {
                case VlcChromecast.Vlc:
                    buttonStateVlc = buttonStateVlcChromcastState;
                    break;
                case VlcChromecast.Chromecast:
                    buttonStateChromecast = buttonStateVlcChromcastState;
                    break;
            }


            switch (buttonStateVlcChromcastState)
            {
                case ButtonStateVlcChromcastState.Init: //Vlc and Chromecast
                    switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player starting...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast starting...";
                            break;
                    }

                    toolStripButtonMediaPreviewPlay.Enabled = true;
                    toolStripButtonMediaPreviewPause.Enabled = false;
                    toolStripButtonMediaPreviewFastBackward.Enabled = false;
                    toolStripButtonMediaPreviewFastForward.Enabled = false;
                    toolStripButtonMediaPreviewStop.Enabled = false;

                    toolStripTraceBarItemMediaPreviewTimer.Enabled = true;

                    break;

                #region Opening
                case ButtonStateVlcChromcastState.Opening: //Vlc and Chromecast
                    PreviewSlideshowWait();

                    switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player opening media...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast opening media...";
                            isChromecasting = true;
                            break;
                    }

                    if (!isChromecasting || (isChromecasting && vlcChromecast == VlcChromecast.Chromecast))
                    {
                        toolStripButtonMediaPreviewPlay.Enabled = false;
                        toolStripButtonMediaPreviewPause.Enabled = false;
                        toolStripButtonMediaPreviewFastBackward.Enabled = false;
                        toolStripButtonMediaPreviewFastForward.Enabled = false;
                        toolStripButtonMediaPreviewStop.Enabled = false;

                        toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
                    }
                    break;
                #endregion 

                #region Playing
                case ButtonStateVlcChromcastState.Playing: //Vlc and Chromecast
                    switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player playing...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast playing...";
                            isChromecasting = true;
                            break;
                    }

                    if (!isChromecasting || (isChromecasting && vlcChromecast == VlcChromecast.Chromecast))
                    {

                        if (canPlayAndPause) isPlayingVideoEndReached = true;
                        else isPlayingVideoEndReached = false;

                        toolStripButtonMediaPreviewPlay.Enabled = false;
                        toolStripButtonMediaPreviewPause.Enabled = true;
                        if (!isChromecasting && canPlayAndPause)
                        {
                            toolStripButtonMediaPreviewFastBackward.Enabled = true;
                            toolStripButtonMediaPreviewFastForward.Enabled = true;
                        }
                        else
                        {
                            toolStripButtonMediaPreviewFastBackward.Enabled = false;
                            toolStripButtonMediaPreviewFastForward.Enabled = false;
                        }

                        toolStripButtonMediaPreviewStop.Enabled = true;

                        toolStripTraceBarItemMediaPreviewTimer.Enabled = true;
                    }

                    break;
                #endregion

                #region Paused
                case ButtonStateVlcChromcastState.Paused: //Vlc and Chromecast
                    switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player paused...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast paused...";
                            isChromecasting = true;
                            break;
                    }

                    if (!isChromecasting || (isChromecasting && vlcChromecast == VlcChromecast.Chromecast))
                    {
                        if (!isChromecasting || canPlayAndPause)
                        {
                            isPlayingVideoEndReached = false; //Video paused
                            toolStripButtonMediaPreviewPlay.Enabled = true;
                        }
                        else
                        {
                            isPlayingVideoEndReached = true; //Picture
                            toolStripButtonMediaPreviewPlay.Enabled = false;
                            PreviewSlideshowNextTimer(true);
                        }

                        toolStripButtonMediaPreviewPause.Enabled = false;
                        toolStripButtonMediaPreviewFastBackward.Enabled = false;
                        toolStripButtonMediaPreviewFastForward.Enabled = false;

                        if (isChromecasting) toolStripButtonMediaPreviewStop.Enabled = true;
                        else toolStripButtonMediaPreviewStop.Enabled = false;

                        toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
                    }
                    break;
                #endregion

                #region Cancelled
                case ButtonStateVlcChromcastState.Cancelled: //Chromecast
                    toolStripLabelMediaPreviewStatus.Text = "Chromecast command cancelled...";
                    break;
                #endregion

                #region Stopped
                case ButtonStateVlcChromcastState.Stopped: //Vlc
                    toolStripLabelMediaPreviewStatus.Text = "VLC player stopped...";
                    toolStripButtonMediaPreviewPlay.Enabled = false;
                    toolStripButtonMediaPreviewPause.Enabled = false;
                    toolStripButtonMediaPreviewFastBackward.Enabled = false;
                    toolStripButtonMediaPreviewFastForward.Enabled = false;
                    toolStripButtonMediaPreviewStop.Enabled = false;
                    toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
                    break;
                #endregion

                #region Interrupted
                case ButtonStateVlcChromcastState.Interrupted: //Chromecast
                    //toolStripLabelMediaPreviewStatus.Text = "Chromecast command interrupted...";
                    break;
                #endregion

                #region EndReached
                case ButtonStateVlcChromcastState.EndReached: //Vlc and Chromecast
                    PreviewSlideshowNextTimer(false);

                    switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player video end reached...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast video end reached...";
                            isChromecasting = true;
                            break;
                    }

                    if (!isChromecasting || (isChromecasting && vlcChromecast == VlcChromecast.Chromecast))
                    {
                        isPlayingVideoEndReached = true;
                        toolStripButtonMediaPreviewPlay.Enabled = true;
                        toolStripButtonMediaPreviewPause.Enabled = false;
                        toolStripButtonMediaPreviewFastBackward.Enabled = false;
                        toolStripButtonMediaPreviewFastForward.Enabled = false;
                        toolStripButtonMediaPreviewStop.Enabled = true;
                        toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
                    }
                    break;
                #endregion

                #region Connecting
                case ButtonStateVlcChromcastState.Connecting: //Chromecast
                    toolStripLabelMediaPreviewStatus.Text = "Chromecast command connection...";
                    isChromecasting = false;
                    break;
                #endregion

                #region Connected
                case ButtonStateVlcChromcastState.Connected: //Chromecast
                    toolStripLabelMediaPreviewStatus.Text = "Chromecast command connected...";
                    isChromecasting = true;
                    break;
                #endregion

                #region Disconnected
                case ButtonStateVlcChromcastState.Disconnected: //Chromecast
                    isChromecasting = false;
                    toolStripLabelMediaPreviewStatus.Text = "Chromecast command disconnected...";

                    toolStripButtonMediaPreviewPlay.Enabled = false;
                    toolStripButtonMediaPreviewPause.Enabled = false;
                    toolStripButtonMediaPreviewFastBackward.Enabled = false;
                    toolStripButtonMediaPreviewFastForward.Enabled = false;
                    toolStripButtonMediaPreviewStop.Enabled = false;
                    toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
                    break;
                #endregion 

                #region Buffering
                case ButtonStateVlcChromcastState.Buffering: //Vlc and Chromecast
                    switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player buffering...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast buffering...";
                            break;
                    }
                    break;
                #endregion

                #region Backward
                case ButtonStateVlcChromcastState.Backward: //Vlc
                    toolStripLabelMediaPreviewStatus.Text = "VLC player backward...";
                    break;
                #endregion

                #region Forward
                case ButtonStateVlcChromcastState.Forward: //Vlc
                    toolStripLabelMediaPreviewStatus.Text = "VLC player forward...";
                    break;
                #endregion

                #region VolumeChanged
                case ButtonStateVlcChromcastState.VolumeChanged: //Vlc and Chromecast
                    /*switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player volume changed to " + ((int)vlcVolume * 100).ToString() + "%...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast volume changed to " + ((int)chromcastVolume * 100).ToString() + "%...";
                            break;
                    }*/
                    break;
                #endregion

                #region Mute
                case ButtonStateVlcChromcastState.Mute: //Vlc and Chromecast
                    /*
                    switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player muted...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast muted...";
                            break;
                    }*/
                    break;
                #endregion

                #region Unmute
                case ButtonStateVlcChromcastState.Unmute: //Vlc and Chromecast
                    /*switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player unmuted...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast unmuted...";
                            break;
                    }*/
                    break;
                #endregion

                #region TimeChanged
                case ButtonStateVlcChromcastState.TimeChanged:
                    if (stopwachMediaTimeChanged.IsRunning && stopwachMediaTimeChanged.ElapsedMilliseconds > 300)
                    {
                        stopwachMediaTimeChanged.Restart(); //Update only after 300ms

                        if (!isChromecasting || (isChromecasting && vlcChromecast == VlcChromecast.Chromecast))
                        {
                            toolStripLabelMediaPreviewTimer.Text = ConvertMsToHuman((long)lastKnownChromeCastCurrentTime * 1000);
                        }
                        else
                        {
                            if (vlcMediaPlayerVideoView.Length == -1) toolStripLabelMediaPreviewTimer.Text = "Timer: No video";
                            else toolStripLabelMediaPreviewTimer.Text = ConvertMsToHuman(vlcTime) + "/" + ConvertMsToHuman(vlcMediaPlayerVideoView.Length);
                        }
                        /*switch (vlcChromecast)
                        {
                            case VlcChromecast.Vlc:
                                if (vlcMediaPlayerVideoView.Length == -1) toolStripLabelMediaPreviewTimer.Text = "Timer: No video";
                                else toolStripLabelMediaPreviewTimer.Text = ConvertMsToHuman(vlcTime) + "/" + ConvertMsToHuman(vlcMediaPlayerVideoView.Length);
                                break;

                            case VlcChromecast.Chromecast:
                                toolStripLabelMediaPreviewTimer.Text = ConvertMsToHuman((long)lastKnownChromeCastCurrentTime * 1000);
                                break;
                        }*/
                    }
                    break;
                #endregion

                #region PositionChanged - Vlc % played
                case ButtonStateVlcChromcastState.PositionChanged:
                    if (stopwachVlcMediaPositionChanged.IsRunning && stopwachVlcMediaPositionChanged.ElapsedMilliseconds > 300)
                    {
                        stopwachVlcMediaPositionChanged.Restart();


                        if (vlcMediaPlayerVideoView.Length == -1)
                        {
                            toolStripTraceBarItemMediaPreviewTimerUpdating = true;
                            toolStripTraceBarItemMediaPreviewTimer.TrackBar.SuspendLayout();


                            toolStripTraceBarItemMediaPreviewTimer.TrackBar.Value = 0;
                            toolStripTraceBarItemMediaPreviewTimer.TrackBar.ResumeLayout();
                            toolStripTraceBarItemMediaPreviewTimerUpdating = false;
                        }
                        else
                        {
                            toolStripTraceBarItemMediaPreviewTimerUpdating = true;
                            toolStripTraceBarItemMediaPreviewTimer.TrackBar.SuspendLayout();


                            toolStripTraceBarItemMediaPreviewTimer.Value = (int)(vlcPosition * 100);
                            toolStripTraceBarItemMediaPreviewTimer.TrackBar.ResumeLayout();
                            toolStripTraceBarItemMediaPreviewTimerUpdating = false;
                        }
                    }
                    break;
                #endregion

                #region Error
                case ButtonStateVlcChromcastState.Error:
                    switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player error encountered...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast error encountered...";
                            break;
                    }
                    toolStripButtonMediaPreviewPlay.Enabled = false;
                    toolStripButtonMediaPreviewPause.Enabled = false;
                    toolStripButtonMediaPreviewFastBackward.Enabled = false;
                    toolStripButtonMediaPreviewFastForward.Enabled = false;
                    toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
                    break;
                    #endregion
            }

        }
        #endregion 

        #endregion


        #region VlcMediaplayer - Events 

        #region VlcMediaplayer - Pause
        private void VlcMediaPlayerVideoView_Paused(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Paused, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - Opening
        private void VlcMediaPlayerVideoView_Opening(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Opening, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - EndReached
        private void VlcMediaPlayerVideoView_EndReached(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.EndReached, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - Error
        private void VlcMediaPlayerVideoView_EncounteredError(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Error, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - Buffering
        private void VlcMediaPlayerVideoView_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Buffering, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - Stopped
        private void VlcMediaPlayerVideoView_Stopped(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Stopped, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - Playing
        private void VlcMediaPlayerVideoView_Playing(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Playing, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - Backward
        private void VlcMediaPlayerVideoView_Backward(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Backward, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - Forward
        private void VlcMediaPlayerVideoView_Forward(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Forward, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - VolumeChanged
        private void VlcMediaPlayerVideoView_VolumeChanged(object sender, MediaPlayerVolumeChangedEventArgs e)
        {
            vlcVolume = e.Volume;
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.VolumeChanged, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - Muted
        private void VlcMediaPlayerVideoView_Muted(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Mute, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - Unmuted
        private void VlcMediaPlayerVideoView_Unmuted(object sender, EventArgs e)
        {
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Unmute, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - TimeChanged
        Stopwatch stopwachMediaTimeChanged = new Stopwatch();
        private void VlcMediaPlayerVideoView_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            vlcTime = e.Time;
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.TimeChanged, VlcChromecast.Vlc);
        }
        #endregion

        #region VlcMediaplayer - PositionChanged
        private void VlcMediaPlayerVideoView_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            vlcPosition = e.Position;
            SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.PositionChanged, VlcChromecast.Vlc);
        }
        #endregion

        #endregion

        #region Vlc Chromecast *** NOT IN USE ***
        //LibVLCSharp.Shared.RendererItem selectedVlcRendererItem = null;
        #region Vlc Chromecast - Chromecast Device Discoverer - Deleted
        private void _rendererDiscoverer_ItemDeleted(object sender, RendererDiscovererItemDeletedEventArgs e)
        {
            if (vlcRendererItems.Contains(e.RendererItem)) vlcRendererItems.Remove(e.RendererItem);
        }
        #endregion

        #region Vlc Chromecast - Chromecast Device Discoverer - Added
        private void _rendererDiscoverer_ItemAdded(object sender, RendererDiscovererItemAddedEventArgs e)
        {
            //Console.WriteLine($"New item discovered: {e.RendererItem.Name} of type {e.RendererItem.Type}");
            //if (e.RendererItem.CanRenderVideo) Console.WriteLine("Can render video");
            //if (e.RendererItem.CanRenderAudio) Console.WriteLine("Can render audio");
            //Console.WriteLine("Chromecast icon: " + (e.RendererItem.IconUri == null ? "" : e.RendererItem.IconUri));
            // add newly found renderer item to local collection
            if (e.RendererItem.CanRenderVideo && !vlcRendererItems.Contains(e.RendererItem)) vlcRendererItems.Add(e.RendererItem);
        }
        #endregion 

        #endregion 

        #region Preview - MediaButton Action

        #region Preview - MediaButton Action - Play
        private void toolStripButtonMediaPreviewPlay_Click(object sender, EventArgs e)
        {

            if (vlcMediaPlayerVideoView.Length != -1)
            {
                if (!vlcMediaPlayerVideoView.WillPlay)
                {
                    if (previewItems.Count > 0) ShowPreviewItem(previewItems[previewMediaindex]);
                }
                else
                {

                    if (canPlayAndPause)
                    {
                        vlcMediaPlayerVideoView.Play();

                        if (isPlayingVideoEndReached) ShowPreviewItem(previewItems[previewMediaindex]);
                        else GoogleCast_Play();
                    }
                }
            }
        }
        #endregion

        #region Preview - MediaButton Action - Pause
        private void toolStripButtonMediaPreviewPause_Click(object sender, EventArgs e)
        {
            if (canPlayAndPause)
            {
                if (videoView1.MediaPlayer.CanPause) videoView1.MediaPlayer.Pause();
                GoogleCast_Pause();
            }
        }
        #endregion

        #region Preview - MediaButton Action - Stop
        private void PreviewStop()
        {
            SlideShowInit(0);
            GoogleCast_Stop(true);
            if (videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Stop();
        }
        private void toolStripButtonMediaPreviewStop_Click(object sender, EventArgs e)
        {
            PreviewStop();
        }
        #endregion 

        #region Preview - MediaButton Action -  Close
        private void toolStripButtonMediaPreviewClose_Click(object sender, EventArgs e)
        {
            isChromecastCloseClicked = true;
            timerFindGoogleCast.Stop();
            PreviewStop();
            panelMediaPreview.Visible = false;

        }
        #endregion

        #region Preview - MediaButton Action - SeekPosition ValueChanged 
        private bool toolStripTraceBarItemMediaPreviewTimerUpdating = false;
        private void toolStripTraceBarItemSeekPosition_ValueChanged(object sender, EventArgs e)
        {
            if (toolStripTraceBarItemMediaPreviewTimerUpdating) return;
            if (videoView1.MediaPlayer.IsSeekable) vlcMediaPlayerVideoView.Position = (float)toolStripTraceBarItemMediaPreviewTimer.TrackBar.Value / 100;
        }
        #endregion

        #region Preview - MediaButton Action - FastBackward_Click
        private void toolStripButtonMediaPreviewFastBackward_Click(object sender, EventArgs e)
        {
            vlcMediaPlayerVideoView.Time -= 10000;
        }
        #endregion

        #region Preview - MediaButton Action - FastForward_Click
        private void toolStripButtonMediaPreviewFastForward_Click(object sender, EventArgs e)
        {
            vlcMediaPlayerVideoView.Time += 10000;
        }
        #endregion

        #region Preview - MediaButton Action - ShowPreviewItem
        private void ShowPreviewItem(string fullFilename)
        {
            ShowMediaChromecast();

            if (videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Stop();

            if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilename))
            {
                canPlayAndPause = true;
                try
                {
                    if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(new Media(_libVLC, fullFilename, FromType.FromPath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                videoView1.Visible = true;
                imageBoxPreview.Visible = false;
            }
            if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilename))
            {
                canPlayAndPause = false;

                toolStripButtonMediaPreviewPlay.Enabled = false;
                toolStripButtonMediaPreviewPause.Enabled = false;
                toolStripButtonMediaPreviewFastBackward.Enabled = false;
                toolStripButtonMediaPreviewFastForward.Enabled = false;
                toolStripTraceBarItemMediaPreviewTimer.Enabled = false;

                try
                {
                    imageBoxPreview.Image = ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.LoadImage(fullFilename);
                    PreviewSlideshowNextTimer(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                imageBoxPreview.Visible = true;
                imageBoxPreview.ZoomToFit();

                videoView1.Visible = false;

            }
        }
        #endregion 

        #region Preview - MediaButton Action - Previous
        private void toolStripButtonMediaPreviewPrevious_Click(object sender, EventArgs e)
        {
            if (previewItems.Count == 0) return;
            previewMediaindex--;
            if (previewMediaindex < 0) previewMediaindex = previewItems.Count - 1;
            if (previewItems.Count > 0) ShowPreviewItem(previewItems[previewMediaindex]);
        }
        #endregion 

        #region Preview - MediaButton Action - Next
        private void PreviewNext()
        {
            if (previewItems.Count == 0) return;
            previewMediaindex++;
            if (previewMediaindex > previewItems.Count - 1) previewMediaindex = 0;
            if (previewItems.Count > 0) ShowPreviewItem(previewItems[previewMediaindex]);
        }

        private void toolStripButtonMediaPreviewNext_Click(object sender, EventArgs e)
        {
            PreviewNext();
        }
        #endregion

        #region Preview - MediaButton Action - DropDown - Media Selected
        private void ToolStripDropDownItemPreviewMedia_Click(object sender, EventArgs e)
        {
            isChromecastStopClicked = false;
            ToolStripMenuItem clickedToolStripMenuItem = (ToolStripMenuItem)sender;
            previewMediaindex = (int)clickedToolStripMenuItem.Tag;
            ShowPreviewItem(clickedToolStripMenuItem.Text);
        }

        #endregion

        #endregion

        #region SlideShow
        private void SlideShowInit(int intervalMs = 0)
        {
            toolStripMenuItemPreviewSlideShow2sec.Checked = false;
            toolStripMenuItemPreviewSlideShow4sec.Checked = false;
            toolStripMenuItemPreviewSlideShow6sec.Checked = false;
            toolStripMenuItemPreviewSlideShow8sec.Checked = false;
            toolStripMenuItemPreviewSlideShow10sec.Checked = false;
            toolStripMenuItemPreviewSlideShowStop.Checked = false;
            timerPreviewNextTimer.Stop();

            switch (intervalMs)
            {
                case 0:
                    isSlideShowRunning = false;
                    slideShowIntervalMs = 2000;
                    toolStripMenuItemPreviewSlideShowStop.Enabled = false;
                    break;
                case 2000:
                    isSlideShowRunning = true;
                    slideShowIntervalMs = intervalMs;
                    toolStripMenuItemPreviewSlideShow2sec.Checked = true;
                    toolStripMenuItemPreviewSlideShowStop.Enabled = true;
                    PreviewNext();
                    break;
                case 4000:
                    isSlideShowRunning = true;
                    slideShowIntervalMs = intervalMs;
                    toolStripMenuItemPreviewSlideShow4sec.Checked = true;
                    toolStripMenuItemPreviewSlideShowStop.Enabled = true;
                    PreviewNext();
                    break;
                case 6000:
                    isSlideShowRunning = true;
                    slideShowIntervalMs = intervalMs;
                    toolStripMenuItemPreviewSlideShow6sec.Checked = true;
                    toolStripMenuItemPreviewSlideShowStop.Enabled = true;
                    PreviewNext();
                    break;
                case 8000:
                    isSlideShowRunning = true;
                    slideShowIntervalMs = intervalMs;
                    toolStripMenuItemPreviewSlideShow8sec.Checked = true;
                    toolStripMenuItemPreviewSlideShowStop.Enabled = true;
                    PreviewNext();
                    break;
                case 10000:
                    isSlideShowRunning = true;
                    slideShowIntervalMs = intervalMs;
                    toolStripMenuItemPreviewSlideShow10sec.Checked = true;
                    toolStripMenuItemPreviewSlideShowStop.Enabled = true;
                    PreviewNext();
                    break;
            }

        }

        private void toolStripMenuItemPreviewSlideShow2sec_Click(object sender, EventArgs e)
        {
            SlideShowInit(2000);
        }

        private void toolStripMenuItemPreviewSlideShow4sec_Click(object sender, EventArgs e)
        {
            SlideShowInit(4000);
        }

        private void toolStripMenuItemPreviewSlideShow6sec_Click(object sender, EventArgs e)
        {
            SlideShowInit(6000);
        }

        private void toolStripMenuItemPreviewSlideShow8sec_Click(object sender, EventArgs e)
        {
            SlideShowInit(8000);
        }

        private void toolStripMenuItemPreviewSlideShow10sec_Click(object sender, EventArgs e)
        {
            SlideShowInit(10000);
        }

        private void toolStripMenuItemPreviewSlideShowStop_Click(object sender, EventArgs e)
        {
            SlideShowInit(0);
        }

        private void PreviewSlideshowWait()
        {
            timerPreviewNextTimer.Interval = 2000;
            timerPreviewNextTimer.Stop();
        }

        private void PreviewSlideshowNextTimer(bool useTimer)
        {
            if (isSlideShowRunning)
            {
                if (useTimer)
                {
                    timerPreviewNextTimer.Interval = slideShowIntervalMs;
                    timerPreviewNextTimer.Start();
                }
                else PreviewNext();
            }
            else timerPreviewNextTimer.Stop();
        }

        private void timerPreviewNextTimer_Tick(object sender, EventArgs e)
        {
            if (isSlideShowRunning) PreviewNext();
        }
        #endregion
    }
}
