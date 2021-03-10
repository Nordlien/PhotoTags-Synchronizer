using GoogleCast.Models.Media;
using ImageAndMovieFileExtentions;
using LibVLCSharp.Shared;
using NHttp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : Form
    {
        
        #region Preview - globale variables
        private List<string> previewItems = new List<string>();
        private int previewMediaindex = 0;
        private bool canPlayAndPause = false;

        //VLC
        private LibVLC _libVLC;
        private MediaPlayer vlcMediaPlayerVideoView = null;   
        
        //Preview help varaibles
        private long vlcTime = 0;
        private float vlcPosition = 0;
        private float vlcVolume = 1;
        private float chromcastVolume = 1;
        private double rotateDegress = 0;

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

            PreviewInitVlcChromecast();
            PreviewInitVlc();
            PreviewInitControls();
            PreviewSlideshowWait();
        }
        #endregion

        #region Vlc Chromecast
        private RendererDiscoverer vlcRendererDiscoverer;
        private readonly List<RendererItem> _vlcRendererItems = new List<RendererItem>();
        private List<LibVLCSharp.Shared.RendererItem> vlcRendererItems = new List<LibVLCSharp.Shared.RendererItem>();
        //private  MediaPlayer vlc_mediaPlayer;

        private void PreviewInitVlcChromecast()
        {
            #region Vlc Find Chromecast devices
            RendererDescription vlcRendererDescription;
            vlcRendererDescription = _libVLC.RendererList.FirstOrDefault(r => r.Name.Equals("microdns_renderer"));

            vlcRendererDiscoverer = new RendererDiscoverer(_libVLC, vlcRendererDescription.Name);
            vlcRendererDiscoverer.ItemAdded += VlcRendererDiscoverer_ItemAdded;
            vlcRendererDiscoverer.ItemDeleted += VlcRendererDiscoverer_ItemDeleted;
            vlcRendererDiscoverer.Start();
            #endregion 
        }

        #region Vlc Chromecast - Chromecast Device Discoverer - Added

        private void VlcRendererDiscoverer_ItemDeleted(object sender, RendererDiscovererItemDeletedEventArgs e)
        {
            if (vlcRendererItems.Contains(e.RendererItem)) vlcRendererItems.Remove(e.RendererItem);
        }
        #endregion 

        #region Vlc Chromecast - Chromecast Device Discoverer - Added

        private void VlcRendererDiscoverer_ItemAdded(object sender, RendererDiscovererItemAddedEventArgs e)
        {
            Console.WriteLine($"New item discovered: {e.RendererItem.Name} of type {e.RendererItem.Type}");
            if (e.RendererItem.CanRenderVideo && !vlcRendererItems.Contains(e.RendererItem)) vlcRendererItems.Add(e.RendererItem);

        }
        #endregion
        
        private void VlcSetChromecastRender(string selectedChromecast,  string fullFilename)
        {
            //videoView1.MediaPlayer.Play(new LibVLCSharp.Shared.Media(_libVLC, fullFilename, FromType.FromPath));

            if (!vlcRendererItems.Any())
            {
                MessageBox.Show("No chromecast items found. Abort casting...");
                return;
            }

            foreach (LibVLCSharp.Shared.RendererItem rendererItem in vlcRendererItems)
            {
                if (rendererItem.Name == selectedChromecast) videoView1.MediaPlayer.SetRenderer(rendererItem);
            }
            //LibVLCSharp.Shared.Media media = new LibVLCSharp.Shared.Media(_libVLC, fullFilename, FromType.FromPath);
            //videoView1.MediaPlayer.Play(media);
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
            SetRotateDegress(0);

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
                Preview_LoadAndShowItem(previewItems[previewMediaindex]);
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

        #region GoogleCast - Load media and Play
        private async void GoogleCast_LoadMediaAndPlay(string contentSource, string fullFilename)
        {
            if (await ConnectMediaChannelAsync() && IsApplicationStarted())
            {
                if (mediaPlaying != contentSource)
                {
                    //GoogleCast.Models.Media.MediaStatus googleCast_CurrentMediaStatus = null;

                    /*string contentType = "";
                    if (ImageAndMovieFileExtentionsUtility.IsImageFormat(fileExtention)) contentType = mimeFormatImage;
                    if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(fileExtention)) contentType = mimeFormatVideo;*/

            string contentType = System.Web.MimeMapping.GetMimeMapping(fullFilename);
                    try
                    {
                        mediaPlaying = contentSource;
                        GoogleCast.Models.Media.MediaStatus googleCast_CurrentMediaStatus = await googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>().LoadAsync(new MediaInformation() { ContentType = contentType, ContentId = contentSource });
                        if (googleCast_CurrentMediaStatus != null) mediaPlaying = "";
                        else MessageBox.Show("Chromecast failed to load media:\r\n" + contentSource);
                    }
                    catch (Exception ex)
                    {
                        mediaPlaying = "";
                        GoogleCast_Stop(true);
                        MessageBox.Show("Communication with Chromecast failed... \r\n" + contentSource + "\r\nError message:" + ex.Message);
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
                        GoogleCast_Stop(true);
                        MessageBox.Show("Communication with Chromecast failed continue play... \r\n" + ex.Message);
                    }
                }

            }
            else
            {
                GoogleCast_Stop(true);
                MessageBox.Show("Communication with Chromecast failed to reconnect...");
                //googleCast_IsMediaChannelConnected = false;
            }
        }
        #endregion

        #region GoogleCast - Play (Resume Play)
        private async void GoogleCast_ResumePlay()
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

            Preview_LoadAndShowCurrentIndex();
        }
        #endregion

        #region ShowMediaShromcast
        private bool WillStartChromecast()
        {
            if (googleCast_SelectedReceiver == null) return false;
            if (isChromecastStopClicked) return false;
            if (isChromecastCloseClicked) return false;
            return true;
        }
        private void ShowMediaChromecast(string playItem)
        {
            if (!WillStartChromecast()) return;

            SelectedDevice(googleCast_SelectedReceiver);

            GoogleCast_LoadMediaAndPlay(playItem, previewItems[previewMediaindex]);
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
                    string outputImageExtention = (Properties.Settings.Default.ChromecastImageOutputFormat == "Original" ?
                        Path.GetExtension(mediaFullFilename) : 
                        Properties.Settings.Default.ChromecastImageOutputFormat);

                    string mimeFormatImage = System.Web.MimeMapping.GetMimeMapping(mediaFullFilename);

                    mediaByteArray = ImageAndMovieFileExtentionsUtility.LoadAndConvertImage(mediaFullFilename, outputImageExtention,
                        Properties.Settings.Default.ChromecastImageOutputResolutionWidth,
                        Properties.Settings.Default.ChromecastImageOutputResolutionHeight, rotateDegress);
                    e.Response.CacheControl = "";
                    e.Response.ContentType = mimeFormatImage;
                }
                else if (ImageAndMovieFileExtentionsUtility.IsVideoFormat(mediaFullFilename))
                {
                    string outputVideoExtention = Path.GetExtension(mediaFullFilename);

                    string mimeFormatVideo = System.Web.MimeMapping.GetMimeMapping(mediaFullFilename); ;
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


            //ButtonStateVlcChromcastState buttonPreviousStateVlc = buttonStateVlc;
            //ButtonStateVlcChromcastState buttonPreviousStateChromecast = buttonStateChromecast;

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
                #region Init
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

                    toolStripButtonMediaPreviewPlay.Enabled = false;
                    toolStripButtonMediaPreviewPause.Enabled = false;
                    toolStripButtonMediaPreviewFastBackward.Enabled = false;
                    toolStripButtonMediaPreviewFastForward.Enabled = false;
                    toolStripButtonMediaPreviewStop.Enabled = false;

                    toolStripTraceBarItemMediaPreviewTimer.Enabled = true;

                    break;
                #endregion


                #region Opening / Loading
                case ButtonStateVlcChromcastState.Opening: //Vlc and Chromecast
                    PreviewSlideshowWait();

                    switch (vlcChromecast)
                    {
                        case VlcChromecast.Vlc:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player loading media...";
                            break;
                        case VlcChromecast.Chromecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast loading media...";
                            isChromecasting = true;
                            break;
                        case VlcChromecast.Image:
                            toolStripLabelMediaPreviewStatus.Text = "Image loading...";
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
                        case VlcChromecast.Image:
                            toolStripLabelMediaPreviewStatus.Text = "Image loaded...";
                            break;
                    }

                    if (!isChromecasting || (isChromecasting && vlcChromecast == VlcChromecast.Chromecast))
                    {
                  
                        isPlayingVideoEndReached = true;
                        toolStripButtonMediaPreviewPlay.Enabled = (vlcChromecast != VlcChromecast.Image);
                        toolStripButtonMediaPreviewPause.Enabled = false;
                        toolStripButtonMediaPreviewFastBackward.Enabled = false;
                        toolStripButtonMediaPreviewFastForward.Enabled = false;
                        toolStripButtonMediaPreviewStop.Enabled = (vlcChromecast != VlcChromecast.Image);
                        toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
                    }

                    
                    break;
                #endregion

                #region Connecting
                case ButtonStateVlcChromcastState.Connecting: //Chromecast
                    toolStripLabelMediaPreviewStatus.Text = "Connecting to chromecsat...";
                    isChromecasting = false;
                    break;
                #endregion

                #region Connected
                case ButtonStateVlcChromcastState.Connected: //Chromecast
                    toolStripLabelMediaPreviewStatus.Text = "Chromecast connected...";
                    isChromecasting = true;
                    break;
                #endregion

                #region Disconnected
                case ButtonStateVlcChromcastState.Disconnected: //Chromecast
                    isChromecasting = false;
                    toolStripLabelMediaPreviewStatus.Text = "Chromecast disconnected...";

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

        #region Preview - MediaButton Action

        #region Preview - MediaButton Action - Rotate
        private void SetRotateDegress(double newRotateDegress)
        {
            rotateDegress = newRotateDegress;
            switch (rotateDegress)
            {
                case 0:
                    toolStripButtonMediaPreviewRotateCW.Checked = false;
                    toolStripButtonMediaPreviewRotate180.Checked = false;
                    toolStripButtonMediaPreviewRotateCCW.Checked = false;
                    break;
                case 90:
                    toolStripButtonMediaPreviewRotateCW.Checked = true;
                    toolStripButtonMediaPreviewRotate180.Checked = false;
                    toolStripButtonMediaPreviewRotateCCW.Checked = false;
                    break;
                case 180:
                    toolStripButtonMediaPreviewRotateCW.Checked = false;
                    toolStripButtonMediaPreviewRotate180.Checked = true;
                    toolStripButtonMediaPreviewRotateCCW.Checked = false;
                    break;
                case 270:
                    toolStripButtonMediaPreviewRotateCW.Checked = false;
                    toolStripButtonMediaPreviewRotate180.Checked = false;
                    toolStripButtonMediaPreviewRotateCCW.Checked = true;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void toolStripButtonMediaPreviewRotateCCW_Click(object sender, EventArgs e)
        {
            if (toolStripButtonMediaPreviewRotateCCW.Checked) 
                SetRotateDegress(0);
            else
                SetRotateDegress(270);
            Preview_LoadAndShowCurrentIndex();
        }

        private void toolStripButtonMediaPreviewRotate180_Click(object sender, EventArgs e)
        {
            if (toolStripButtonMediaPreviewRotate180.Checked)
                SetRotateDegress(0);
            else
                SetRotateDegress(180);
            Preview_LoadAndShowCurrentIndex();
        }

        private void toolStripButtonMediaPreviewRotateCW_Click(object sender, EventArgs e)
        {
            if (toolStripButtonMediaPreviewRotateCW.Checked)
                SetRotateDegress(0);
            else
                SetRotateDegress(90);
            Preview_LoadAndShowCurrentIndex();
        }
        #endregion

        #region Preview - MediaButton Action - Play
        private void PreviewResumePlay()
        {
            if (vlcMediaPlayerVideoView.Length != -1)
            {
                if (!vlcMediaPlayerVideoView.WillPlay)
                {
                    if (previewItems.Count > 0) Preview_LoadAndShowItem(previewItems[previewMediaindex]);
                }
                else
                {

                    if (canPlayAndPause)
                    {
                        if (isPlayingVideoEndReached) Preview_LoadAndShowItem(previewItems[previewMediaindex]);
                        else
                        {
                            GoogleCast_ResumePlay();
                            vlcMediaPlayerVideoView.Play();
                        }
                    }
                }
            } 
        }

        private void toolStripButtonMediaPreviewPlay_Click(object sender, EventArgs e)
        {
            PreviewResumePlay();
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

        #region Preview - MediaButton Action - Previous
        private void toolStripButtonMediaPreviewPrevious_Click(object sender, EventArgs e)
        {
            if (previewItems.Count == 0) return;
            previewMediaindex--;
            if (previewMediaindex < 0) previewMediaindex = previewItems.Count - 1;
            //if (previewItems.Count > 0) Preview_LoadAndShowItem(previewItems[previewMediaindex]);
            SetRotateDegress(0);
            Preview_LoadAndShowCurrentIndex();
        }
        #endregion 

        #region Preview - MediaButton Action - Next
        private void PreviewNext()
        {
            if (previewItems.Count == 0) return;
            previewMediaindex++;
            if (previewMediaindex > previewItems.Count - 1) previewMediaindex = 0;
            //if (previewItems.Count > 0) Preview_LoadAndShowItem(previewItems[previewMediaindex]);
            SetRotateDegress(0);
            Preview_LoadAndShowCurrentIndex();
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
            //Preview_LoadAndShowItem(clickedToolStripMenuItem.Text);
            SetRotateDegress(0);
            Preview_LoadAndShowCurrentIndex();
        }

        #endregion

        #region Preview - MediaButton Action - Show 
        private void Preview_LoadAndShowCurrentIndex()
        {
            if (previewMediaindex < 0) previewMediaindex = previewItems.Count - 1;
            if (previewMediaindex > previewItems.Count - 1) previewMediaindex = 0;
            if (previewItems.Count > 0) Preview_LoadAndShowItem(previewItems[previewMediaindex]);
        }

        private void Preview_LoadAndShowItem(string fullFilename)
        {
            if (videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Stop();

            string playItem =
                    String.Format("http://{0}", nHttpServer.EndPoint) + "/chromecast?index=" + previewMediaindex +
                    "&rotate=" + rotateDegress.ToString() +
                    "&loadmedia=" +
                    System.Web.HttpUtility.UrlEncode(previewItems[previewMediaindex]);

            if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilename))
            {
                canPlayAndPause = true;
                try
                {
                    LibVLCSharp.Shared.Media media = new LibVLCSharp.Shared.Media(_libVLC, fullFilename, FromType.FromPath);
                    switch (Properties.Settings.Default.ChromecastContainer)
                    {
                        case "Original":
                            ShowMediaChromecast(playItem);
                            if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(media);
                            break;

                        case "VLC":
                            string friendlyName = googleCast_SelectedReceiver.FriendlyName;

                            GoogleCast_Stop(true);
                            //SelectedDevice(null);

                            //if (WillStartChromecast()) 
                            VlcSetChromecastRender(friendlyName, "");
                            if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(media);
                            break;

                        default:
                            string option = "sout=#transcode{" + "" +
                                "vcodec=" + Properties.Settings.Default.ChromecastVideoCodec +
                                (Properties.Settings.Default.ChromecastVideoBitrate < 1000 ? "" : ",vb=" + (Properties.Settings.Default.ChromecastVideoBitrate / 1000)) +
                                (Properties.Settings.Default.ChromecastImageOutputResolutionWidth < 1 ? "" : ",width=" + Properties.Settings.Default.ChromecastVideoOutputResolutionWidth) +
                                //(Properties.Settings.Default.ChromecastImageOutputResolutionHeight < 1 ? "" : ",height=" + Properties.Settings.Default.ChromecastVideoOutputResolutionHeight) +
                                (Properties.Settings.Default.ChromecastAudioCodec == "Original" ? "" : ",acodec=" + Properties.Settings.Default.ChromecastAudioCodec) +
                                //(Properties.Settings.Default.ChromecastAudioBitrate < 1000 ? "" : ",ab=" + (Properties.Settings.Default.ChromecastAudioBitrate / 1000)) +
                                //(Properties.Settings.Default.ChromecastAudioSampleRate < 1 ? "" : ",samplerate=" + Properties.Settings.Default.ChromecastAudioSampleRate) +
                                ",channels=2" +
                                ",mux=" + Properties.Settings.Default.ChromecastContainer + "}:http{dst=:8080/webcam.ogg}";
                            Console.WriteLine(option);
                            //option = "sout=#transcode{vcodec=mp4v,vb=800,acodec=mpga,ab=128,channels=2,mux=ogg}:http{dst=:8080/webcam.ogg}";
                            media.AddOption(option);

                            if (rotateDegress != 0)
                            {
                                //-sout="#transcode{vfilter="transform{transform-type=270}",vcodec=h264,vb=2500,scale=0.5,acodec=aac,ab=128,channels=2}:standard{access=file,mux=mp4,dst="output.mp4"}" vlc://quit
                                //media.AddOption("--transform-type=" + ((int)rotateDegress).ToString());    //transform-type ：90、180、270、hflip、vflip、transpose、antitranspose
                                //media.AddOption("--video-filter=transform");
                                //media.AddOption(":sout=#standard{filter=transform,transform-type=" + ((int)rotateDegress).ToString() + "}");
                                //vfilter=canvas{width=852,height=480}
                                media.AddOption(":sout=#display{vfilter=transform{transform-type=" + ((int)rotateDegress).ToString() + "}}");
                                media.AddOption(":sout -keep");
                            }

                            //if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(new Media(_libVLC, fullFilename, FromType.FromPath));
                            if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(media);
                            ShowMediaChromecast("http://192.168.86.163:8080/webcam.ogg");

                            break;
                            #region Help
                            /*
                            Media container formats
                                MP2T, MP3, MP4, OGG, WAV, WebM 

                            vcodec=
                            acodec=
                            mux=


                            stream output
                            standard    allows to send the stream via an access output module : for example, UDP, file, HTTP, … You will probably want to use this module at the end of your chains.
                            transcode   is used to transcode (decode and re-encode the stream using a different codec and/or bitrate) the audio and the video of the input stream. If the input or output access method doesn't allow pace control (network, capture devices), this will be done "on the fly", in real time. This can require quite a lot of CPU power, depending on the parameters set. Other streams, such as files and disks, are transcoded as fast as the system allows it.
                            duplicate   allows you to create a second chain, where the stream will be handled in an independent way.
                            display     allows you to display the input stream, as VLC would normally do. Used with the duplicate module, this allows you to monitor the stream while processing it.
                            rtp         streams over RTP (one UDP port for each elementary stream). This module also allows RTSP support.

                            --sout "#module1{option1=parameter1{parameter-option1},option2=parameter2}:module2{option1=…,option2=…}:…"
                            */
                            //static const vlc_fourcc_t DEFAULT_TRANSCODE_AUDIO = VLC_CODEC_MP3;
                            //static const vlc_fourcc_t DEFAULT_TRANSCODE_VIDEO = VLC_CODEC_H264;
                            //OGG sout=#transcode{vcodec=theo,vb=1024,channels=1,ab=128,samplerate=44100,width=320}:http{dst=:8080/webcam.ogg}
                            //MP4 dshow: --dshow-adev=none --dshow-caching=0 --sout=#transcode{vcodec=h264,vb=1024,channels=1,ab=128,samplerate=44100,width=320}:http{mux=ts,dst=:8080/webcam.mp4} 

                            //you're telling VLC to stream in TS format mux=ts this is your problem, you need to mux in mp4
                            //codecs=theora, mux=ogg

                            //vlc.exe rtsp://192.168.0.53/ :network-caching=1000 :sout=#transcode{vcodec=theo,vb=1600,scale=1,acodec=none}:http{mux=ogg,dst=:8181/stream} :no-sout-rtp-sap :no-sout-standard-sap :sout-keep
                            //vlc.exe http://ksportiptv.com:1557/SajidLhr/Cj6cZT2p6V/1345 :network-caching=1000 :sout=#transcode{vcodec=theo,vb=1600,scale=1,acodec=none}:http{mux=ogg,dst=:8181/stream} :no-sout-rtp-sap :no-sout-standard-sap :sout-kee

                            //#transcode{vcodec=h264,
                            //  venc=x264{profile=baseline,level=3,preset=ultrafast,tune=zerolatency,crf=28,keyint=50},
                            //  acodec=aac,ab=128,channels=2,samplerate=44100}:
                            //  rtp{sdp=rtsp://:9090/desktop.sdp}
                            //  #transcode{vcodec=h264,venc=x264{profile=baseline,level=3,preset=ultrafast,tune=zerolatency,crf=28,keyint=50},
                            //  acodec=aac,ab=128,channels=2,samplerate=44100}:
                            //  rtp{sdp=rtsp://:9090/desktop.sdp}
                            //#transcode{vfilter=canvas{width=852,height=480},vcodec=h264,venc=x264{profile=baseline,level=3,preset=ultrafast,tune=zerolatency,keyint=50},acodec=aac,ab=128,channels=2,samplerate=44100}:rtp{sdp=rtsp://:9090/desktop.sdp}

                            //#transcode{vcodec=mpgv,vb=3500,fps=25,acodec=mpga,ab=128,channels=2,deinterlace,
                            //  vfilter=canvas{no-padd,height=576,width=720,aspect=4:3}}:std{access=udp,mux=ts,dst=...}

                            /*
                            Properties.Settings.Default.ChromecastAudioBitrate; //E.g.44100
                            Properties.Settings.Default.ChromecastAudioCodec; //E.g. mp4a
                            Properties.Settings.Default.ChromecastImageOutputFormat; //E.g. .JPEG

                            Properties.Settings.Default.ChromecastImageOutputResolutionWidth, 
                            Properties.Settings.Default.ChromecastImageOutputResolutionHeight);

                             //E.g. 7500 kbps
                            ); //E.g. h264

                            Properties.Settings.Default.ChromecastVideoOutputResolutionWidth
                            Properties.Settings.Default.ChromecastVideoOutputResolutionHeight
                            */




                            //media.AddOption(":sout=#transcode{vcodec=h264,vb=0,scale=0,acodec=acc,ab=128,channels=2,samplerate=44100}");
                            //media.AddOption(":sout=#http{dst=192.168.86.163,port=8080,mime=video/x-matroska,mux=matroska,sdp=http://192.168.86.163:8080}");
                            //media.AddOption(":sout=#http{dst=192.168.86.163,port=8080,mime=video/mp4,mux=mp4,sdp=http://192.168.86.163:8080}");

                            //media.AddOption(":sout=#transcode{vcodec=h264,vb=0,scale=0,acodec=mp4a,ab=128,channels=2,samplerate=44100}:http{dst=192.168.86.163,port=8080,sdp=http://192.168.86.163:8080}");
                            //media.AddOption(":sout -keep");

                            //transform-type <string> { "90", "180", "270", "hflip", "vflip", "transpose", "antitranspose" } : Transformation type default value: "90"
                            //sout=#transcode{vcodec=VP8,vb=4000,acodec=mpga,channels=2,mux=ogg}:http{dst=:8080/webcam.ogg}

                            //VideoOrientation
                            //videoView1.MediaPlayer.Or;
                            #endregion

                    } 
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
                ShowMediaChromecast(playItem);

                canPlayAndPause = false;

                SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Opening, VlcChromecast.Image);

                try
                {
                    imageBoxPreview.Image = 
                        ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.LoadImageAndRotate(fullFilename, rotateDegress);
                    PreviewSlideshowNextTimer(true);
                    SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.EndReached, VlcChromecast.Image);
                }
                catch (Exception ex)
                {
                    SetButtonStateVlcChromcastInit(ButtonStateVlcChromcastState.Error, VlcChromecast.Image);
                    MessageBox.Show(ex.Message);
                }
                imageBoxPreview.Visible = true;
                imageBoxPreview.ZoomToFit();

                videoView1.Visible = false;

            }
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
