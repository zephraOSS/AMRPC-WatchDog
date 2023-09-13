using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AMRPC_WatchDog_Desktop
{
    public class Payload
    {
        private string _playerStateValue;
        private double _endTimeValue = -1;
        private double _duration = -1;

        public event PropertyChangedEventHandler PropertyChanged;

        public static class ResponseTypes
        {
            public const string Response = "res";
            public const string Event = "event";
        }
        
        public static class PlayingStatuses
        {
            public const string Playing = "playing";
            public const string NotStarted = "not_started";
            public const string Paused = "paused";
        }

        public string title { get; set; }
        public string album { get; set; }
        public string artist { get; set; }
        public string thumbnailPath { get; set; }
        public string type { get; set; }

        public string playerState
        {
            get => _playerStateValue;
            set
            {
                if (value == _playerStateValue) return;
                _playerStateValue = value;
                NotifyPropertyChanged();
            }
        }

        public double endTime
        {
            get => _endTimeValue;
            set
            {
                if (value == _endTimeValue) return;
                _endTimeValue = value;
                NotifyPropertyChanged();
            }
        }
        
        public double duration
        {
            get => _duration;
            set
            {
                if (value == _duration) return;
                _duration = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ResetToInitialState()
        {
            title = null;
            artist = null;
            album = null;
            thumbnailPath = null;
            playerState = PlayingStatuses.NotStarted;
            endTime = -1;
            duration = -1;
            type = ResponseTypes.Event;
        }
    }
}