using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media.Control;


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

            _payload.playerState = playbackInfo.PlaybackStatus.ToString().ToLower() == Payload.PlayingStatuses.Playing 
                    ? Payload.PlayingStatuses.Playing : Payload.PlayingStatuses.Paused;

            Double newEndTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() +
                                timelineProperties.EndTime.TotalMilliseconds;
            _payload.endTime = newEndTime;
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

        private void ParseMediaProperties(GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
        {
            _payload.artist = mediaProperties.AlbumArtist.Split('-').First().Trim();
            _payload.album = mediaProperties.AlbumArtist.Split('—').Last().Trim();
            // payload.ThumbnailPath = mediaProperties.Thumbnail;
            _payload.title = mediaProperties.Title;
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

        private static async Task<GlobalSystemMediaTransportControlsSessionMediaProperties> GetMediaProperties(GlobalSystemMediaTransportControlsSession AMPSession) =>
            AMPSession == null ? null : await AMPSession.TryGetMediaPropertiesAsync();
    }
}