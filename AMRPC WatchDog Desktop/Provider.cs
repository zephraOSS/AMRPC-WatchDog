using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Media.Control;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;


namespace AMRPC_WatchDog_Desktop
{
    internal class Provider
    {
        private GlobalSystemMediaTransportControlsSessionManager _sessionManager;
        private GlobalSystemMediaTransportControlsSession _ampSession;

        private const string AMPModelId = "AppleInc.AppleMusicWin";

        private Messenger _messenger;
        private readonly Payload _payload;

        public Provider() 
        {
            _payload = new Payload();
            _messenger = new Messenger(_payload);

            UpdateSessionManager();
            UpdateAMPSession();
        }

        private async void UpdateSessionManager()
        {
            if (_sessionManager != null) return;
            
            _sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            _sessionManager.SessionsChanged += OnSessionsChanged;
            GetAMPSession();
        }

        private void UpdateAMPSession()
        {
            UpdateSessionManager();
            GetAMPSession();
        }

        private void GetAMPSession()
        {
            if (_sessionManager != null) 
            {
                GlobalSystemMediaTransportControlsSession newSession = FindAMPSession();
                SetAMPSession(newSession);
            }

            if (_ampSession != null)
            {
                _ampSession.MediaPropertiesChanged += OnMediaPropertiesChanged;
                _ampSession.PlaybackInfoChanged += OnPlaybackInfoChanged;
                OnMediaPropertiesChanged(null, null);
            } else
            {
                _payload.ResetToInitialState();
            }
        }

        private void OnPlaybackInfoChanged(GlobalSystemMediaTransportControlsSession sender, PlaybackInfoChangedEventArgs args)
        {
            var playbackInfo = _ampSession.GetPlaybackInfo();
            var timelineProperties = _ampSession.GetTimelineProperties();

            _payload.PlayingStatus = playbackInfo.PlaybackStatus.ToString().ToLower() == Payload.PlayingStatuses.Playing 
                    ? Payload.PlayingStatuses.Playing : Payload.PlayingStatuses.Paused;

            Double newEndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() +
                                timelineProperties.EndTime.TotalMilliseconds;
            _payload.EndTime = newEndTime;
        }

        private async void OnMediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties = await GetMediaProperties(_ampSession);
            if (mediaProperties != null && !(string.IsNullOrEmpty(mediaProperties.Title) && string.IsNullOrEmpty(mediaProperties.AlbumArtist)))
            {
                ParseMediaProperties(mediaProperties);
            }
            else
            {
                _payload.ResetToInitialState();
            }
        }

        private async Task ParseMediaProperties(GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
        {
            _payload.Artist = mediaProperties.AlbumArtist.Split('—').First().Trim();
            _payload.Album = mediaProperties.AlbumArtist.Split('—').Last().Trim();
            _payload.Title = mediaProperties.Title;
            _payload.ThumbnailPath = await LoadImage(mediaProperties);
            OnPlaybackInfoChanged(null, null);
        }

        private void SetAMPSession(GlobalSystemMediaTransportControlsSession newSession)
        {
            if (newSession == null && _ampSession != null)
            {
                _ampSession = null;
            } 
            else if (_ampSession == null)
            {
                _ampSession = newSession;
            }
        }

        private GlobalSystemMediaTransportControlsSession FindAMPSession()
        {
            IReadOnlyList<GlobalSystemMediaTransportControlsSession> sessions = _sessionManager.GetSessions();
            foreach (GlobalSystemMediaTransportControlsSession session in sessions)
            {
                if (session.SourceAppUserModelId.Contains(AMPModelId))
                {
                    return session;
                }
            }

            return null;
        }

        private void OnSessionsChanged(object sender, SessionsChangedEventArgs e) 
        {
            UpdateAMPSession();
        }

        private static async Task<string> LoadImage(GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
        {
            var executableDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            Directory.CreateDirectory($"{executableDirectory}\\thumbnails");
            
            var img = Image.FromStream((await mediaProperties.Thumbnail.OpenReadAsync()).AsStream());
            var fileName = $"{mediaProperties.Title.Replace(" ", "_")}_-_{mediaProperties.AlbumArtist.Split('—').First().Trim().Replace(" ", "_")}.jpg";
            var savingPath = $"thumbnails\\{fileName}";
            
            img.Save(savingPath);
            return $"{executableDirectory}\\{savingPath}";
        }

        private static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties> GetMediaProperties(GlobalSystemMediaTransportControlsSession AMPSession) =>
            AMPSession == null ? null : await AMPSession.TryGetMediaPropertiesAsync();
    }
}