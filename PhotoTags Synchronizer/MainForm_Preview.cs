﻿using GoogleCast.Models.Media;
using ImageAndMovieFileExtentions;
using LibVLCSharp.Shared;
using NHttp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;

namespace PhotoTagsSynchronizer
{
    public partial class MainForm : KryptonForm
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        #region Preview - globale variables
        private List<string> previewMediaItemFilenames = new List<string>();
        private int previewMediaShowItemIndex = 0;
        private bool previewMediaIsCurrentMediaVideo = false;
        private bool isGooglecasting = false;

        //VLC
        private LibVLC _libVLC; //Init in MainForm().cs  

        //Preview help varaibles
        private long vlcTime = 0;
        private float vlcPosition = 0;
        private float vlcVolume = 1;
        private float chromcastVolume = 1;
        private double rotateDegress = 0;
        Stopwatch stopwachVlcMediaPositionChanged = new Stopwatch();
        private IPEndPoint vlcChromecastIPEndPoint = null;
        #endregion

        #region IsChromecastSelected - Turn On / Off
        private bool IsChromecastSelected
        {
            get { return googleCast_SelectedReceiver != null; }
        }

        private void ChromecastingSwitchOn(GoogleCast.IReceiver selectedReceiver)
        {
            googleCast_SelectedReceiver = selectedReceiver;
        }
        private void ChromecastingSwitchOff()
        {
            googleCast_SelectedReceiver = null;
        }
        #endregion

        #region Preview Media -- Click  ---
        private void MediaPreviewInit(List<string> listOfMediaFiles, string imageListViewLastHoverFullfilename)
        {
            isGooglecasting = false;
            isGooglecastDisconnectedStarted = false;

            SetRotateDegress(0);
            SlideShowInit();

            //GooglecastInitSender(); 
            GooglecastFindReceiversAsync();

            vlcChromecastIPEndPoint = new IPEndPoint(nHttpServer.EndPoint.Address, GetOpenPort());
            VlcChromecastRenderDiscover();
            VlcMediaPlayerInit();

            PreviewInitControls(listOfMediaFiles, imageListViewLastHoverFullfilename);
            PreviewSlideshowWait();
        }
        #endregion

        #region Vlc Chromecast
        private RendererDiscoverer vlcRendererDiscoverer;
        private readonly List<RendererItem> _vlcRendererItems = new List<RendererItem>();
        private List<LibVLCSharp.Shared.RendererItem> vlcRendererItems = new List<LibVLCSharp.Shared.RendererItem>();

        #region Vlc Chromecast - Init Thread - Find Chromecast devices
        private void VlcChromecastRenderDiscover()
        {
            RendererDescription vlcRendererDescription;
            vlcRendererDescription = _libVLC.RendererList.FirstOrDefault(r => r.Name.Equals("microdns_renderer"));

            vlcRendererDiscoverer = new RendererDiscoverer(_libVLC, vlcRendererDescription.Name);
            vlcRendererDiscoverer.ItemAdded += VlcRendererDiscoverer_ItemAdded;
            vlcRendererDiscoverer.ItemDeleted += VlcRendererDiscoverer_ItemDeleted;
            vlcRendererDiscoverer.Start();

        }
        #endregion 

        #region Vlc Chromecast - Chromecast Device Discoverer - Delete

        private void VlcRendererDiscoverer_ItemDeleted(object sender, RendererDiscovererItemDeletedEventArgs e)
        {
            if (vlcRendererItems.Contains(e.RendererItem)) vlcRendererItems.Remove(e.RendererItem);
        }
        #endregion 

        #region Vlc Chromecast - Chromecast Device Discoverer - Added
        private void VlcRendererDiscoverer_ItemAdded(object sender, RendererDiscovererItemAddedEventArgs e)
        {
            Logger.Trace($"New item discovered: {e.RendererItem.Name} of type {e.RendererItem.Type}");            
            if (e.RendererItem.CanRenderVideo && !vlcRendererItems.Contains(e.RendererItem)) vlcRendererItems.Add(e.RendererItem);

        }
        #endregion

        #region Vlc Chromecast - Set Vlc Chromecast Render
        private void VlcSetChromecastRender(GoogleCast.IReceiver selectedChromecast)
        {
            if (!vlcRendererItems.Any())
            {
                MessageBox.Show("No chromecast items found. Abort casting...");
                return;
            }

            string friendlyName = googleCast_SelectedReceiver?.FriendlyName;

            bool renderFound = false;
            foreach (LibVLCSharp.Shared.RendererItem rendererItem in vlcRendererItems)
            {
                if (rendererItem.Name == friendlyName)
                {
                    if (rendererItem.IconUri.Contains(googleCast_SelectedReceiver?.IPEndPoint.Address.ToString()))
                    {
                        renderFound = true;
                        videoView1.MediaPlayer.SetRenderer(rendererItem);
                    }
                }
            }
            if (!renderFound) MessageBox.Show("Can connect LibVlc Chromecast render, render not found");
        }
        #endregion 

        #endregion

        #region Preview - Init - Vlc MediaPlayer
        private void VlcMediaPlayerInit()
        {
            videoView1.MediaPlayer.EnableKeyInput = true;
            videoView1.MediaPlayer.EnableHardwareDecoding = true;
            videoView1.MediaPlayer.EnableKeyInput = true;

            videoView1.MediaPlayer.Backward += VlcMediaPlayerVideoView_Backward;
            videoView1.MediaPlayer.Forward += VlcMediaPlayerVideoView_Forward;
            videoView1.MediaPlayer.Buffering += VlcMediaPlayerVideoView_Buffering;
            videoView1.MediaPlayer.EncounteredError += VlcMediaPlayerVideoView_EncounteredError;

            videoView1.MediaPlayer.Muted += VlcMediaPlayerVideoView_Muted;
            videoView1.MediaPlayer.Opening += VlcMediaPlayerVideoView_Opening;

            videoView1.MediaPlayer.Paused += VlcMediaPlayerVideoView_Paused;
            videoView1.MediaPlayer.PositionChanged += VlcMediaPlayerVideoView_PositionChanged;

            videoView1.MediaPlayer.EndReached += VlcMediaPlayerVideoView_EndReached;
            videoView1.MediaPlayer.Playing += VlcMediaPlayerVideoView_Playing;

            videoView1.MediaPlayer.Stopped += VlcMediaPlayerVideoView_Stopped;

            videoView1.MediaPlayer.TimeChanged += VlcMediaPlayerVideoView_TimeChanged;
            videoView1.MediaPlayer.Unmuted += VlcMediaPlayerVideoView_Unmuted;
            videoView1.MediaPlayer.VolumeChanged += VlcMediaPlayerVideoView_VolumeChanged;

        }
        #endregion

        #region Preview - Init - Controls
        private void PreviewInitControls(List<string> listOfMediaFiles, string imageListViewLastHoverFullfilename)
        {
            toolStripDropDownButtonChromecastList.DropDownItems.Clear();
            toolStripDropDownButtonChromecastList.Enabled = false;

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

            previewMediaShowItemIndex = 0;
            previewMediaItemFilenames.Clear();
            toolStripDropDownButtonMediaList.DropDownItems.Clear();

            foreach (string fileFullPath in listOfMediaFiles)
            {
                previewMediaItemFilenames.Add(fileFullPath);
                if (fileFullPath == imageListViewLastHoverFullfilename) previewMediaShowItemIndex = previewMediaItemFilenames.Count - 1;
                ToolStripMenuItem toolStripDropDownItem = new ToolStripMenuItem();
                toolStripDropDownItem.Click += ToolStripDropDownItemPreviewMedia_Click;
                toolStripDropDownItem.Text = fileFullPath;
                toolStripDropDownItem.Tag = previewMediaItemFilenames.Count - 1;
                toolStripDropDownButtonMediaList.DropDownItems.Add(toolStripDropDownItem);
            }

            if (previewMediaItemFilenames.Count > 0)
            {
                
                Preview_LoadAndShowItem(previewMediaItemFilenames[previewMediaShowItemIndex]);
            }
        }
        #endregion 

        #region Preview - GoogleCast - Init sender vaiables
        private bool GooglecastInitSender()
        {
            
            if (googleCast_sender == null && !isGooglecastDisconnectedStarted)
            {
                googleCast_sender = new GoogleCast.Sender();
                googleCast_sender.Disconnected += GoogleCast_sender_Disconnected;
                googleCast_sender.GetChannel<GoogleCast.Channels.IMediaChannel>().StatusChanged += GoogleCast_mediaChannel_StatusChanged;
                googleCast_sender.GetChannel<GoogleCast.Channels.IReceiverChannel>().StatusChanged += GoogleCast_ReceiverChannel_StatusChanged;
                MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Init, MediaPlaybackEventsSource.Googlecast);
                googleCast_IsChromecastReceiverConnected = false;
                return true;
            }
            else 
                return !isGooglecastDisconnectedStarted;
        }
        #endregion 

        #region Preview - GoogleCast - FindReceiversAsync
        private async void GooglecastFindReceiversAsync()
        {
            timerFindGoogleCast.Stop();

            // Use the DeviceLocator to find a Chromecast
            googleCast_DeviceLocator = new GoogleCast.DeviceLocator();

            try
            {
                googleCast_receivers = await googleCast_DeviceLocator.FindReceiversAsync();
            } catch (Exception ex)
            {
                MessageBox.Show("Was not able to start serivce FindReceiversAsync\r\n" + ex.Message);
            }

            toolStripDropDownButtonChromecastList.Enabled = false;
            //toolStripDropDownButtonChromecastList.DropDownItems.Clear();

            foreach (ToolStripItem toolStripItem in toolStripDropDownButtonChromecastList.DropDownItems) toolStripItem.Enabled = false;

            foreach (GoogleCast.IReceiver googleCast_receiver in googleCast_receivers)
            {
                bool itemFound = false;
                foreach (ToolStripItem toolStripItem in toolStripDropDownButtonChromecastList.DropDownItems)
                {
                    if ((toolStripItem.Tag as GoogleCast.IReceiver)?.Id == googleCast_receiver.Id)
                    {
                        toolStripItem.Enabled = true;
                        itemFound = true;
                    }
                }
                if (!itemFound)
                {
                    ToolStripMenuItem toolStripDropDownItem = new ToolStripMenuItem();
                    toolStripDropDownItem.Click += ToolStripDropDownItemPreviewChromecast_Click;
                    toolStripDropDownItem.Text = googleCast_receiver.FriendlyName;
                    toolStripDropDownItem.Tag = googleCast_receiver;
                    toolStripDropDownButtonChromecastList.DropDownItems.Add(toolStripDropDownItem);
                }
            }

            ToolStripDropDownCheckReceiver(googleCast_SelectedReceiver);
            if (toolStripDropDownButtonChromecastList.DropDownItems.Count > 0) toolStripDropDownButtonChromecastList.Enabled = true;
            else toolStripDropDownButtonChromecastList.Enabled = false;

            timerFindGoogleCast.Start();
        }

        private void timerFindGoogleCast_Tick(object sender, EventArgs e)
        {
            GooglecastFindReceiversAsync();
        }
        #endregion

        #region GoogleCast - Internal Commands
        //https://github.com/kakone/GoogleCast
        private GoogleCast.DeviceLocator googleCast_DeviceLocator = null;
        private IEnumerable<GoogleCast.IReceiver> googleCast_receivers = null;
        private GoogleCast.IReceiver googleCast_ConnectedReceiver = null;
        private GoogleCast.IReceiver googleCast_SelectedReceiver = null;
        private GoogleCast.Sender googleCast_sender;
        private bool isGooglecastDisconnectedStarted = false;

        private string googleCast_CurrentMediaUrlPlaying = "";
        private string googleCast_LastKnownErrorMessage = "";

        #region GoogleCast - IsChromecastReceiverConnected
        bool googleCast_IsChromecastReceiverConnected = false;
        private bool GoogleCast_IsChromecastReceiverConnected
        {
            get
            {
                if (googleCast_sender == null) return false;
                return googleCast_IsChromecastReceiverConnected;//|| GoogleCast_IsMediaChannelStarted;
            }
            set
            {
                googleCast_IsChromecastReceiverConnected = value;
            }
        }
        #endregion

        #region GoogleCast - GoogleCast_IsMediaChannelStarted
        private bool GoogleCast_IsMediaChannelStarted
        {
            get
            {
                if (googleCast_sender == null) return false;
                GoogleCast.Channels.IMediaChannel mediaChannel = googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>();
                return googleCast_sender != null && mediaChannel?.Status != null && string.IsNullOrEmpty(mediaChannel?.Status?.FirstOrDefault()?.IdleReason);
            }
        }
        #endregion

        #region GoogleCast - IsApplicationStarted
        private bool GoogleCast_IsApplicationChannelStarted
        {
            get
            {
                if (googleCast_sender == null) return false;
                GoogleCast.Channels.IReceiverChannel receiverChannel = googleCast_sender?.GetChannel<GoogleCast.Channels.IReceiverChannel>();
                return receiverChannel != null && receiverChannel?.Status != null && receiverChannel?.Status.Applications != null;
            }
        }
        #endregion

        #region GoogleCast - (ReConnectReceiverAsync) ConnectMediaChannel - Async
        private async Task<bool> GoogleCast_ConnectMediaChannelAsync(GoogleCast.IReceiver receiver)
        {
            try
            {
                if (!GoogleCast_IsApplicationChannelStarted) await googleCast_sender.LaunchAsync(googleCast_sender.GetChannel<GoogleCast.Channels.IMediaChannel>());
                return true;
            }
            catch (Exception ex)
            {
                googleCast_LastKnownErrorMessage = "Connect with Chromecast appliaction failed... \r\n" + ex.Message;
                return false;
            }
        }
        #endregion

        #region GoogleCast - Command - PlayUrl
        private async void GoogleCast_Command_PlayUrl(string playItemUrl, string fullFilename)
        {
            bool retry;
            bool errorOccured;
            
            do
            {
                retry = false;
                errorOccured = false;

                if (GooglecastInitSender() &&
                    await GoogleCast_ConnectChromecastReceiver(googleCast_SelectedReceiver) &&
                    await GoogleCast_ConnectMediaChannelAsync(googleCast_ConnectedReceiver) 
                   )
                {
                    if (googleCast_CurrentMediaUrlPlaying != playItemUrl)
                    {
                        #region contentType 
                        string contentType = System.Web.MimeMapping.GetMimeMapping(fullFilename);

                        switch (Path.GetExtension(fullFilename).ToLower())
                        {
                            case ".flv": contentType = "video/x-flv"; break; //Flash	
                            case ".mp4": contentType = "video/mp4"; break; //MPEG-4
                            case ".m3u8": contentType = "application/x-mpegURL"; break; //iPhone Index	
                            case ".ts": contentType = "video/MP2T"; break; //iPhone Segment	
                            case ".3gp": contentType = "video/3gpp"; break; //3GP Mobile	
                            case ".mov": contentType = "video/quicktime"; break; //QuickTime	
                            case ".avi": contentType = "video/x-msvideo"; break; //A/V Interleave	
                            case ".wmv": contentType = "video/x-ms-wmv"; break; //Windows Media	
                            case ".ogg": contentType = "video/ogg"; break; //
                        }
                        #endregion

                        try
                        {
                            googleCast_CurrentMediaUrlPlaying = playItemUrl;
                            GoogleCast.Models.Media.MediaStatus googleCast_CurrentMediaStatus = await googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>().LoadAsync(new MediaInformation() { ContentType = contentType, ContentId = playItemUrl });
                            if (googleCast_CurrentMediaStatus != null) googleCast_CurrentMediaUrlPlaying = "";
                            else 
                            {
                                errorOccured = false;
                                googleCast_LastKnownErrorMessage = "Chromecast failed to load media:\r\n" + playItemUrl;
                            }
                        }
                        catch (Exception ex)
                        {
                            googleCast_CurrentMediaUrlPlaying = "";
                            errorOccured = false;
                            googleCast_LastKnownErrorMessage = "Communication with Chromecast failed... \r\n" + playItemUrl + "\r\nError message:" + ex.Message;
                        }
                    }
                    else
                    {
                        try
                        {
                            googleCast_CurrentMediaUrlPlaying = playItemUrl;
                            await googleCast_sender.GetChannel<GoogleCast.Channels.IMediaChannel>().PlayAsync();
                        }
                        catch (Exception ex)
                        {
                            googleCast_CurrentMediaUrlPlaying = "";
                            errorOccured = false;
                            googleCast_LastKnownErrorMessage = "Communication with Chromecast failed continue play... \r\n" + ex.Message;
                        }
                    }

                }
                else
                {
                    errorOccured = true;
                }

                if (errorOccured)
                {
                    if (MessageBox.Show(googleCast_LastKnownErrorMessage, "Chromecast communcation error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                    {
                        try
                        {
                            await GoogleCast_Disconnect(googleCast_ConnectedReceiver, true);
                            googleCast_sender.Disconnect();
                            googleCast_sender = null;
                        } catch { }

                        retry = true;
                    }
                }
            } while (retry);
        }
        #endregion

        #region GoogleCast - Command - Resume Play
        private async void GoogleCast_Command_ResumePlay()
        {
            try
            {
                if (GooglecastInitSender() &&
                    await GoogleCast_ConnectChromecastReceiver(googleCast_SelectedReceiver) &&
                    await GoogleCast_ConnectMediaChannelAsync(googleCast_ConnectedReceiver)
                   ) await googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>().PlayAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Communication with Google Cast failed to play... \r\n" + ex.Message);
            }
        }
        #endregion 

        #region GoogleCast - Command - Pause
        private async void GoogleCast_Command_Pause()
        {
            try
            {
                if (GooglecastInitSender() &&
                    await GoogleCast_ConnectChromecastReceiver(googleCast_SelectedReceiver) &&
                    await GoogleCast_ConnectMediaChannelAsync(googleCast_ConnectedReceiver)
                   ) await googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>().PauseAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Communication with Google Cast failed to pause... \r\n" + ex.Message);
            }
        }
        #endregion

        #region GoogleCast - Disconnect Application and/or Chromecast
        private async Task<bool> GoogleCast_Disconnect(GoogleCast.IReceiver connectToThisReceiver, bool alsoStopChromecast)
        {
            if (isGooglecastDisconnectedStarted) return true;
            //if (googleCast_ConnectedReceiver == null) return true;
            
            isGooglecastDisconnectedStarted = true;
            bool result = await GoogleCast_DisconnectAllReconnectReceiver(connectToThisReceiver, true, alsoStopChromecast, false);            
            isGooglecastDisconnectedStarted = false;
            return result;
        }
        #endregion

        #region GoogleCast - (Re)connect
        private async Task<bool> GoogleCast_ConnectChromecastReceiver(GoogleCast.IReceiver connectToThisReceiver)
        {            
            return await GoogleCast_DisconnectAllReconnectReceiver(connectToThisReceiver, false, false, true);
        }
        #endregion

        #region GoogleCast - ConnectReceiver - Async
        private async Task<bool> GoogleCast_ReConnectReceiverAsync(GoogleCast.IReceiver selectedReceiver)
        {
            if (googleCast_sender == null || selectedReceiver == null) return false;

            if (!GoogleCast_IsChromecastReceiverConnected)
            {
                try
                {
                    await googleCast_sender.ConnectAsync(selectedReceiver);
                    googleCast_ConnectedReceiver = selectedReceiver;
                    GoogleCast_IsChromecastReceiverConnected = true;
                }
                catch (Exception ex)
                {
                    googleCast_LastKnownErrorMessage = "Connect Receiver with Chromecast failed...\r\n" + ex.Message;
                    //googleCast_ConnectedReceiver = null;
                    //googleCast_IsReceiverConnected = false;
                }
            }
            return GoogleCast_IsChromecastReceiverConnected = true;            
        }
        #endregion

        #region GoogleCast - Select Receiver
        private async Task<bool> GoogleCast_DisconnectAllReconnectReceiver(
            GoogleCast.IReceiver connectToThisReceiver, 
            bool forceDisconnect, bool alsoStopChromecast, bool reconnectReceiver)
        {
            bool didReconnectAndConnectedCompleteWithoutError = true;


            forceDisconnect = (googleCast_ConnectedReceiver?.Id != connectToThisReceiver?.Id || forceDisconnect);

            googleCast_LastKnownErrorMessage = "";
            if (forceDisconnect)
            {
                #region Stop MediaChannel (Chromcast APP within Chromcast, E.g. WebMediaViewer, Netflix, etc.)
                if (GoogleCast_IsMediaChannelStarted)
                {
                    try
                    {
                        await googleCast_sender?.GetChannel<GoogleCast.Channels.IMediaChannel>().StopAsync();
                    } catch (Exception ex)
                    {
                        didReconnectAndConnectedCompleteWithoutError = false;
                        googleCast_LastKnownErrorMessage = ex.Message;
                    }
                    googleCast_CurrentMediaUrlPlaying = "";
                    Task.Delay(600).Wait();
                }
                #endregion

                #region Stop Chromecast (Chromecast Receiver app from running)
                if (alsoStopChromecast)
                {
                    if (!GoogleCast_IsMediaChannelStarted) //Implemented
                    {
                        if (await GoogleCast_ReConnectReceiverAsync(googleCast_ConnectedReceiver)) //Implemented
                        {
                            try
                            {
                                googleCast_sender.GetChannel<GoogleCast.Channels.IMediaChannel>().StatusChanged -= GoogleCast_mediaChannel_StatusChanged;
                                googleCast_sender.GetChannel<GoogleCast.Channels.IReceiverChannel>().StatusChanged -= GoogleCast_ReceiverChannel_StatusChanged;
                                await googleCast_sender.GetChannel<GoogleCast.Channels.IReceiverChannel>().StopAsync();
                            }
                            catch (Exception ex)
                            {
                                didReconnectAndConnectedCompleteWithoutError = false;
                                googleCast_LastKnownErrorMessage = ex.Message;
                            }
                            finally
                            {
                                Task.Delay(400).Wait();
                                googleCast_CurrentMediaUrlPlaying = "";
                                googleCast_sender = null;
                                googleCast_ConnectedReceiver = null;
                            }
                        }
                    }
                    else
                    {
                        googleCast_LastKnownErrorMessage = (googleCast_LastKnownErrorMessage == "" ? "" : "\r\n") + "Was not able to stop chromecast, due to app still running...";
                        didReconnectAndConnectedCompleteWithoutError = false;
                    }
                }
                #endregion 
            }

            if (reconnectReceiver && connectToThisReceiver != null && (googleCast_ConnectedReceiver?.Id != connectToThisReceiver?.Id))
            {
                if (!(await GoogleCast_ReConnectReceiverAsync(connectToThisReceiver)))
                {
                    didReconnectAndConnectedCompleteWithoutError = false;
                    googleCast_LastKnownErrorMessage = (googleCast_LastKnownErrorMessage == "" ? "" : "\r\n") + "Was not able to stop chromecast connect to Chromecast Receiver...";
                }
            }
            return didReconnectAndConnectedCompleteWithoutError;
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
                                MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Cancelled, MediaPlaybackEventsSource.Googlecast);
                                break;
                            case "INTERRUPTED":
                                MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Interrupted, MediaPlaybackEventsSource.Googlecast);
                                break;
                            case "FINISHED":
                                MediaPlayerEventsHandler(ButtonStateVlcChromcastState.EndReached, MediaPlaybackEventsSource.Googlecast);
                                break;
                            default:
                                if (status.ExtendedStatus == null)
                                {
                                    MessageBox.Show("GoogleCast Status Player reason: " + MediaStatusToText(status));
                                    Logger.Trace("GoogleCast_mediaChannel_StatusChanged: " + MediaStatusToText(status, ext));
                                }
                                break;
                        }
                        break;
                    case "BUFFERING":
                        MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Buffering, MediaPlaybackEventsSource.Googlecast);
                        break;
                    case "LOADING":
                        MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Opening, MediaPlaybackEventsSource.Googlecast);
                        break;
                    case "PLAYING":
                        MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Playing, MediaPlaybackEventsSource.Googlecast);
                        break;
                    case "PAUSED":
                        MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Paused, MediaPlaybackEventsSource.Googlecast);
                        break;
                    default:
                        MessageBox.Show("GoogleCast Status Player: " + MediaStatusToText(status));
                        Logger.Trace("GoogleCast_mediaChannel_StatusChanged: " + MediaStatusToText(status, ext));
                        break;
                }
                if (status.ExtendedStatus != null) GoogleCast_StatusChanged(status.ExtendedStatus, ext + 1);
                if (status != null)
                {
                    if (lastKnownChromeCastCurrentTime != status.CurrentTime)
                    {
                        lastKnownChromeCastCurrentTime = status.CurrentTime;
                        MediaPlayerEventsHandler(ButtonStateVlcChromcastState.TimeChanged, MediaPlaybackEventsSource.Googlecast);
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
                    MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Connecting, MediaPlaybackEventsSource.Googlecast);
                else
                    MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Connected, MediaPlaybackEventsSource.Googlecast);

                if (status.Volume.Level != null)
                {
                    chromcastVolume = (float)status.Volume.Level;
                    MediaPlayerEventsHandler(ButtonStateVlcChromcastState.VolumeChanged, MediaPlaybackEventsSource.Googlecast);
                }
                if (status.Volume.IsMuted != null)
                {
                    if ((bool)status.Volume.IsMuted) MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Mute, MediaPlaybackEventsSource.Googlecast);
                    else MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Unmute, MediaPlaybackEventsSource.Googlecast);
                }
            }
        }
        #endregion

        #region GoogleCast - Event - Disconnected 
        private void GoogleCast_sender_Disconnected(object sender, EventArgs e)
        {
            //googleCast_CurrentMediaUrlPlaying = "";
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Disconnected, MediaPlaybackEventsSource.Googlecast);
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

        #region GoogleCast - Actions
        private void ShowMediaChromecast(string playItemUrl)
        {
            if (!IsChromecastSelected) return;
            GoogleCast_Command_PlayUrl(playItemUrl, previewMediaItemFilenames[previewMediaShowItemIndex]);
        }
        #endregion

        #endregion 

        #region nHttpServer
        HttpServer nHttpServer = null;
        private Thread _ThreadHttp = null;
        private AutoResetEvent nHttpServerThreadWaitApplicationClosing = null;

        #region nHttpServer - GetLocalIp 
        public string GetLocalIp()
        {
            string localIP = null;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                try
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localIP = endPoint.Address.ToString();
                } catch
                {                    
                    localIP = IPAddress.Loopback.ToString();
                }
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
            Logger.Info("nHTTP server new state: " + nHttpServer.State.ToString());
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
                if (indexMediaFile > -1 && indexMediaFile < previewMediaItemFilenames.Count) mediaFullFilename = previewMediaItemFilenames[indexMediaFile];
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

            if (mediaByteArray != null && mediaByteArray.Length > 0) e.Response.OutputStream.Write(mediaByteArray, 0, mediaByteArray.Length);
            else
            {
                //e.Response.Status = "404 Not Found";
                Image image = Properties.Resources.error404;
                
                using (var stream = new MemoryStream())
                {
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    
                    e.Response.OutputStream.Write(stream.ToArray(), 0, (int)stream.Length);
                }

                e.Response.ContentType = "image/png";

                if (mediaByteArray == null || mediaByteArray.Length == 0) MessageBox.Show("Was not able to load the meida file: " + mediaFullFilename);
            }
        }
        #endregion

        #endregion

        #region ButtonStates based on events         
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
        enum MediaPlaybackEventsSource
        {
            ScreenVlcMediaPlayer,
            Googlecast,
            ScreenImageViewer
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
        private void MediaPlayerEventsHandler(ButtonStateVlcChromcastState buttonStateVlcChromcastState, MediaPlaybackEventsSource vlcChromecast)
        {
            if (InvokeRequired)
            {
                this.BeginInvoke(new Action<ButtonStateVlcChromcastState, MediaPlaybackEventsSource>(MediaPlayerEventsHandler), buttonStateVlcChromcastState, vlcChromecast);
                return;
            }

            switch (buttonStateVlcChromcastState)
            {
                #region Init
                case ButtonStateVlcChromcastState.Init: //Vlc and Chromecast
                    switch (vlcChromecast)
                    {
                        case MediaPlaybackEventsSource.ScreenVlcMediaPlayer:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player starting...";
                            break;
                        case MediaPlaybackEventsSource.Googlecast:
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
                    isPlayingVideoEndReached = false;
                    switch (vlcChromecast)
                    {
                        case MediaPlaybackEventsSource.ScreenVlcMediaPlayer:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player loading media...";
                            break;
                        case MediaPlaybackEventsSource.Googlecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast loading media...";
                            isGooglecasting = true;
                            break;
                        case MediaPlaybackEventsSource.ScreenImageViewer:
                            toolStripLabelMediaPreviewStatus.Text = "Image loading...";
                            break;
                    }

                    if (!isGooglecasting || (isGooglecasting && vlcChromecast == MediaPlaybackEventsSource.Googlecast))
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
                        case MediaPlaybackEventsSource.ScreenVlcMediaPlayer:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player playing...";
                            break;
                        case MediaPlaybackEventsSource.Googlecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast playing...";
                            isGooglecasting = true;
                            break;
                    }

                    
                    if (!isGooglecasting || (isGooglecasting && vlcChromecast == MediaPlaybackEventsSource.Googlecast))
                    {

                        //if (!previewMediaIsCurrentMediaVideo) isPlayingVideoEndReached = true; else 
                        //isPlayingVideoEndReached = false;

                        toolStripButtonMediaPreviewPlay.Enabled = false;

                        toolStripButtonMediaPreviewPause.Enabled = true;

                        if (!isGooglecasting && previewMediaIsCurrentMediaVideo)
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
                        case MediaPlaybackEventsSource.ScreenVlcMediaPlayer:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player paused...";
                            break;
                        case MediaPlaybackEventsSource.Googlecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast paused...";
                            isGooglecasting = true;
                            break;
                    }

                    if (!isGooglecasting || (isGooglecasting && vlcChromecast == MediaPlaybackEventsSource.Googlecast))
                    {
                        if (!isGooglecasting || previewMediaIsCurrentMediaVideo)
                        {
                            //isPlayingVideoEndReached = false; //Video paused
                            toolStripButtonMediaPreviewPlay.Enabled = true;
                        }
                        else
                        {
                            //isPlayingVideoEndReached = true; //Picture
                            toolStripButtonMediaPreviewPlay.Enabled = false;
                        }

                        toolStripButtonMediaPreviewPause.Enabled = false;
                        toolStripButtonMediaPreviewFastBackward.Enabled = false;
                        toolStripButtonMediaPreviewFastForward.Enabled = false;

                        if (isGooglecasting) toolStripButtonMediaPreviewStop.Enabled = true;
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

                    bool useTimer = false;
                    switch (vlcChromecast)
                    {
                        case MediaPlaybackEventsSource.ScreenVlcMediaPlayer:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player video end reached...";
                            useTimer = false;
                            break;
                        case MediaPlaybackEventsSource.Googlecast:
                            toolStripLabelMediaPreviewStatus.Text = "Chromecast video end reached...";
                            useTimer = false;
                            break;
                        case MediaPlaybackEventsSource.ScreenImageViewer:
                            toolStripLabelMediaPreviewStatus.Text = "Image loaded...";
                            useTimer = true;
                            break;
                    }


                    //if (!isGooglecasting)                    
                    

                    if (!isGooglecasting ||
                        (isGooglecasting && vlcChromecast == MediaPlaybackEventsSource.ScreenImageViewer) ||
                        (isGooglecasting && vlcChromecast == MediaPlaybackEventsSource.Googlecast))
                    {
                        if (!isPlayingVideoEndReached) //Avoid double End Reached
                            PreviewSlideshowNextTimer(useTimer);
                        isPlayingVideoEndReached = true;
                        toolStripButtonMediaPreviewPlay.Enabled = (vlcChromecast != MediaPlaybackEventsSource.ScreenImageViewer);
                        toolStripButtonMediaPreviewPause.Enabled = false;
                        toolStripButtonMediaPreviewFastBackward.Enabled = false;
                        toolStripButtonMediaPreviewFastForward.Enabled = false;
                        toolStripButtonMediaPreviewStop.Enabled = (vlcChromecast != MediaPlaybackEventsSource.ScreenImageViewer);
                        toolStripTraceBarItemMediaPreviewTimer.Enabled = false;
                    }

                    //Remove double when casting


                    break;
                #endregion

                #region Connecting
                case ButtonStateVlcChromcastState.Connecting: //Chromecast
                    toolStripLabelMediaPreviewStatus.Text = "Connecting to chromecsat...";
                    isGooglecasting = false;
                    break;
                #endregion

                #region Connected
                case ButtonStateVlcChromcastState.Connected: //Chromecast
                    toolStripLabelMediaPreviewStatus.Text = "Chromecast connected...";
                    isGooglecasting = true;
                    break;
                #endregion

                #region Disconnected
                case ButtonStateVlcChromcastState.Disconnected: //Chromecast
                    isGooglecasting = false;
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
                        case MediaPlaybackEventsSource.ScreenVlcMediaPlayer:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player buffering...";
                            break;
                        case MediaPlaybackEventsSource.Googlecast:
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

                        if (!isGooglecasting || (isGooglecasting && vlcChromecast == MediaPlaybackEventsSource.Googlecast))
                        {
                            toolStripLabelMediaPreviewTimer.Text = ConvertMsToHuman((long)lastKnownChromeCastCurrentTime * 1000);
                        }
                        else
                        {
                            if (videoView1.MediaPlayer.Length == -1) toolStripLabelMediaPreviewTimer.Text = "Timer: No video";
                            else toolStripLabelMediaPreviewTimer.Text = ConvertMsToHuman(vlcTime) + "/" + ConvertMsToHuman(videoView1.MediaPlayer.Length);
                        }
                        /*switch (vlcChromecast)
                        {
                            case VlcChromecast.Vlc:
                                if (videoView1.MediaPlayer.Length == -1) toolStripLabelMediaPreviewTimer.Text = "Timer: No video";
                                else toolStripLabelMediaPreviewTimer.Text = ConvertMsToHuman(vlcTime) + "/" + ConvertMsToHuman(videoView1.MediaPlayer.Length);
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

                        if (videoView1.MediaPlayer.Length == -1)
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
                        case MediaPlaybackEventsSource.ScreenVlcMediaPlayer:
                            toolStripLabelMediaPreviewStatus.Text = "VLC player error encountered...";
                            break;
                        case MediaPlaybackEventsSource.Googlecast:
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
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Paused, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - Opening
        private void VlcMediaPlayerVideoView_Opening(object sender, EventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Opening, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - EndReached
        private void VlcMediaPlayerVideoView_EndReached(object sender, EventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.EndReached, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - Error
        private void VlcMediaPlayerVideoView_EncounteredError(object sender, EventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Error, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - Buffering
        private void VlcMediaPlayerVideoView_Buffering(object sender, MediaPlayerBufferingEventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Buffering, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - Stopped
        private void VlcMediaPlayerVideoView_Stopped(object sender, EventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Stopped, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - Playing
        private void VlcMediaPlayerVideoView_Playing(object sender, EventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Playing, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - Backward
        private void VlcMediaPlayerVideoView_Backward(object sender, EventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Backward, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - Forward
        private void VlcMediaPlayerVideoView_Forward(object sender, EventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Forward, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - VolumeChanged
        private void VlcMediaPlayerVideoView_VolumeChanged(object sender, MediaPlayerVolumeChangedEventArgs e)
        {
            vlcVolume = e.Volume;
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.VolumeChanged, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - Muted
        private void VlcMediaPlayerVideoView_Muted(object sender, EventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Mute, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - Unmuted
        private void VlcMediaPlayerVideoView_Unmuted(object sender, EventArgs e)
        {
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Unmute, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - TimeChanged
        Stopwatch stopwachMediaTimeChanged = new Stopwatch();
        private void VlcMediaPlayerVideoView_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            vlcTime = e.Time;
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.TimeChanged, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
        }
        #endregion

        #region VlcMediaplayer - PositionChanged
        private void VlcMediaPlayerVideoView_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            vlcPosition = e.Position;
            MediaPlayerEventsHandler(ButtonStateVlcChromcastState.PositionChanged, MediaPlaybackEventsSource.ScreenVlcMediaPlayer);
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
            if (videoView1.MediaPlayer.Length != -1)
            {
                if (!videoView1.MediaPlayer.WillPlay)
                {
                    if (previewMediaItemFilenames.Count > 0) Preview_LoadAndShowItem(previewMediaItemFilenames[previewMediaShowItemIndex]);
                }
                else
                {

                    if (previewMediaIsCurrentMediaVideo)
                    {
                        if (isPlayingVideoEndReached) Preview_LoadAndShowItem(previewMediaItemFilenames[previewMediaShowItemIndex]);
                        else
                        {
                            GoogleCast_Command_ResumePlay();
                            videoView1.MediaPlayer.Play();
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
            if (previewMediaIsCurrentMediaVideo)
            {
                if (videoView1.MediaPlayer.CanPause) videoView1.MediaPlayer.Pause();
                GoogleCast_Command_Pause();
            }
        }
        #endregion

        #region Preview - MediaButton Action - Stop
        private async void PreviewStop()
        {
            SlideShowInit(0); //Slideshow stop

            ChromecastingSwitchOff(); //No chromecast selected
            if (!await GoogleCast_Disconnect(googleCast_ConnectedReceiver, true)) MessageBox.Show(googleCast_LastKnownErrorMessage);
            if (videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Stop(); //Stop Vlc mediaplayer
        }
        private void toolStripButtonMediaPreviewStop_Click(object sender, EventArgs e)
        {
            PreviewStop();
        }
        #endregion 

        #region Preview - MediaButton Action -  Close
        private void toolStripButtonMediaPreviewClose_Click(object sender, EventArgs e)
        {
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
            if (videoView1.MediaPlayer.IsSeekable) videoView1.MediaPlayer.Position = (float)toolStripTraceBarItemMediaPreviewTimer.TrackBar.Value / 100;
        }
        #endregion

        #region Preview - MediaButton Action - FastBackward_Click
        private void toolStripButtonMediaPreviewFastBackward_Click(object sender, EventArgs e)
        {
            videoView1.MediaPlayer.Time -= 10000;
        }
        #endregion

        #region Preview - MediaButton Action - FastForward_Click
        private void toolStripButtonMediaPreviewFastForward_Click(object sender, EventArgs e)
        {
            videoView1.MediaPlayer.Time += 10000;
        }
        #endregion

        #region Preview - MediaButton Action - Previous
        private void toolStripButtonMediaPreviewPrevious_Click(object sender, EventArgs e)
        {
            if (previewMediaItemFilenames.Count == 0) return;
            previewMediaShowItemIndex--;
            if (previewMediaShowItemIndex < 0) previewMediaShowItemIndex = previewMediaItemFilenames.Count - 1;
            //if (previewItems.Count > 0) Preview_LoadAndShowItem(previewItems[previewMediaindex]);
            SetRotateDegress(0);
            Preview_LoadAndShowCurrentIndex();
        }
        #endregion 

        #region Preview - MediaButton Action - Next
        private void PreviewNext()
        {
            if (previewMediaItemFilenames.Count == 0) return;
            previewMediaShowItemIndex++;
            if (previewMediaShowItemIndex > previewMediaItemFilenames.Count - 1) previewMediaShowItemIndex = 0;
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
            ToolStripMenuItem clickedToolStripMenuItem = (ToolStripMenuItem)sender;
            previewMediaShowItemIndex = (int)clickedToolStripMenuItem.Tag;
            SetRotateDegress(0);
            Preview_LoadAndShowCurrentIndex();
        }

        #endregion

        #region Preview - Mark as Checked on selected Receiver
        private void ToolStripDropDownCheckReceiver(GoogleCast.IReceiver receiver)
        {
            foreach (ToolStripMenuItem toolStripDropDownItem in toolStripDropDownButtonChromecastList.DropDownItems)
            {
                if (toolStripDropDownItem.Tag == receiver) toolStripDropDownItem.Checked = true;
                else toolStripDropDownItem.Checked = false;
            }
        }
        #endregion

        #region Chromecast - Selected - Click
        private void ChromecastingSelected(GoogleCast.IReceiver selectedReceiver)
        {            
            ToolStripDropDownCheckReceiver(googleCast_SelectedReceiver);
            ChromecastingSwitchOn(selectedReceiver);
            Preview_LoadAndShowCurrentIndex();
        }
        private void ToolStripDropDownItemPreviewChromecast_Click(object sender, EventArgs e)
        {
            if (previewMediaItemFilenames.Count == 0)
            {
                ToolStripDropDownCheckReceiver(null);
                return;
            }
            
            ToolStripMenuItem clickedToolStripMenuItem = (ToolStripMenuItem)sender;
            clickedToolStripMenuItem.Checked = true;

            ChromecastingSelected((GoogleCast.IReceiver)clickedToolStripMenuItem.Tag);
        }
        #endregion

        #region Preview - Load And Show Current Index 
        private void Preview_LoadAndShowCurrentIndex()
        {
            if (previewMediaShowItemIndex < 0) previewMediaShowItemIndex = previewMediaItemFilenames.Count - 1;
            if (previewMediaShowItemIndex > previewMediaItemFilenames.Count - 1) previewMediaShowItemIndex = 0;
            if (previewMediaItemFilenames.Count > 0) 
                Preview_LoadAndShowItem(previewMediaItemFilenames[previewMediaShowItemIndex]);
        }
        #endregion 

        #region Preview - MediaButton Action - Show 
        private void Preview_LoadAndShowItem(string fullFilename)
        {            
            toolStripButtonMediaPreviewNext.Enabled = false;
            toolStripButtonMediaPreviewPrevious.Enabled = false;

            if (videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Stop();

            string playItem =
                    String.Format("http://{0}", nHttpServer.EndPoint) + "/chromecast?index=" + previewMediaShowItemIndex +
                    "&rotate=" + rotateDegress.ToString() +
                    "&loadmedia=" +
                    System.Web.HttpUtility.UrlEncode(previewMediaItemFilenames[previewMediaShowItemIndex]);

            if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsVideoFormat(fullFilename))
            {
                previewMediaIsCurrentMediaVideo = true;
                try
                {
                    LibVLCSharp.Shared.Media media = new LibVLCSharp.Shared.Media(_libVLC, fullFilename, FromType.FromPath);

                    string chromecastTransporter = "SCREEN";  
                    if (IsChromecastSelected) chromecastTransporter = Properties.Settings.Default.ChromecastTransporter;

                    //Properties.Settings.Default.ChromecastContainer
                    //HTTP - Simple HTTP server, send video as is
                    //VLC-Render - Use VLC own Chromecast stack
                    //VLC-Stream - Use VLC stream and own config
                    switch (chromecastTransporter)
                    {
                        case "SCREEN":
                            #region SCREEN

                            videoView1.MediaPlayer.SetRenderer(null);
                            /*
                            _libVLC = new LibVLC(new string[] {
                                "--avcodec-hw=none",
                                "--video-filter=scene",
                                "--scene-format=bmp",
                                "--scene-prefix=vlcsnap",
                                "--scene-ratio=3"
                                
                                });
                            */

                            //rotateDegress = 90;
                            if (rotateDegress != 0 )
                            {
                                
                                media.AddOption(":avcodec-hw=none");

                                media.AddOption(":aspect-ratio=16:9");
                                media.AddOption(":video-filter=invert");

                                //media.AddOption(":video-filter=scene");
                                //media.AddOption(":scene-format=bmp");
                                //media.AddOption(":scene-prefix=vlcsnap");
                                //media.AddOption(":scene-ratio=3");
                                //":sout=#transcode{vfilter=transform{transform-type=" + ((int)rotateDegress).ToString() + "}}:display"

                                //media.AddOption(":sout=#display{vfilter=transform{transform-type=" + ((int)rotateDegress).ToString() + "}}");
                                media.AddOption(":sout=#transcode{vfilter=transform{transform-type=" + ((int)rotateDegress).ToString() + "}}:display");

                                media.AddOption(":sout -keep");
                            }

                            if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(media);
                            break;
                            #endregion 
                        case "HTTP":
                            #region HTTP
                            ShowMediaChromecast(playItem);
                            if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(media);
                            break;
                            #endregion
                        case "VLC-Render":
                            #region VLC-Render
                            
                            VlcSetChromecastRender(googleCast_SelectedReceiver);
                            if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(media);
                            break;
                        #endregion 
                        case "VLC-Stream":
                            #region VLC-Stream
                            string chromecastAgruments = Properties.Settings.Default.ChromecastAgruments;
                            string chromecastUrl = Properties.Settings.Default.ChromecastUrl;
                            string chromecastAudioCodec = Properties.Settings.Default.ChromecastAudioCodec;
                            string chromecastVideoCodec = Properties.Settings.Default.ChromecastVideoCodec;

                            chromecastUrl = chromecastUrl.Replace("{port}", vlcChromecastIPEndPoint.Port.ToString());
                            chromecastUrl = chromecastUrl.Replace("{ipaddress}", vlcChromecastIPEndPoint.Address.ToString());
                            
                            chromecastAgruments = chromecastAgruments.Replace("{vcodec}", chromecastVideoCodec);
                            chromecastAgruments = chromecastAgruments.Replace("{acodec}", chromecastAudioCodec);
                            chromecastAgruments = chromecastAgruments.Replace("{url}", chromecastUrl);

                            Logger.Trace(chromecastAgruments);                            
                            media.AddOption(chromecastAgruments);

                            
                            videoView1.MediaPlayer.SetRenderer(null);
                            videoView1.MediaPlayer.EnableHardwareDecoding = false;
                            
                            //if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(new Media(_libVLC, fullFilename, FromType.FromPath));
                            if (!videoView1.MediaPlayer.IsPlaying) videoView1.MediaPlayer.Play(media);
                            Logger.Trace("http://" + (chromecastUrl.StartsWith(":") ? vlcChromecastIPEndPoint.Address.ToString() : "") + chromecastUrl);
                            ShowMediaChromecast("http://" + (chromecastUrl.StartsWith(":") ? vlcChromecastIPEndPoint.Address.ToString() : "") + chromecastUrl);
                             
                            break;
                        #endregion 
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
                        default:
                            throw new NotImplementedException();

                    } 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Playing Video file failed: " + ex.Message);
                }
                videoView1.Visible = true;
                imageBoxPreview.Visible = false;
            }
            if (ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.IsImageFormat(fullFilename))
            {
                ShowMediaChromecast(playItem);

                previewMediaIsCurrentMediaVideo = false;

                MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Opening, MediaPlaybackEventsSource.ScreenImageViewer);

                try
                {
                    imageBoxPreview.Image = ImageAndMovieFileExtentions.ImageAndMovieFileExtentionsUtility.LoadImageAndRotate(fullFilename, rotateDegress);    
                    MediaPlayerEventsHandler(ButtonStateVlcChromcastState.EndReached, MediaPlaybackEventsSource.ScreenImageViewer);
                }
                catch (Exception ex)
                {
                    MediaPlayerEventsHandler(ButtonStateVlcChromcastState.Error, MediaPlaybackEventsSource.ScreenImageViewer);
                    MessageBox.Show("Playing Image file failed: " + ex.Message);
                }
                imageBoxPreview.Visible = true;
                imageBoxPreview.ZoomToFit();

                videoView1.Visible = false;

            }

            toolStripButtonMediaPreviewNext.Enabled = true;
            toolStripButtonMediaPreviewPrevious.Enabled = true;
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
            isNextMediaTiggered = true;
            timerPreviewNextTimer.Interval = 2000;
            timerPreviewNextTimer.Stop();
        }

        private void PreviewSlideshowNextTimer(bool useTimer)
        {
            if (isSlideShowRunning)
            {
                if (useTimer)
                {
                    isNextMediaTiggered = false;
                    timerPreviewNextTimer.Interval = slideShowIntervalMs;
                    timerPreviewNextTimer.Start();
                }
                else 
                    PreviewNext();
            }
            else timerPreviewNextTimer.Stop();
        }

        bool isNextMediaTiggered = false;
        private void timerPreviewNextTimer_Tick(object sender, EventArgs e)
        {
            if (isNextMediaTiggered) return; //Avoid loop
            isNextMediaTiggered = true;
            PreviewSlideshowWait();
            if (isSlideShowRunning) PreviewNext();
        }
        #endregion
    }
}