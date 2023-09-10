using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AMRPC_WatchDog_Desktop
{
    public class Payload
    {
        private string _playingStateValue;
        private double _endTimeValue = -1;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public string playingState
        {
            get => _playingStateValue;
            set
            {
                if (value == _playingStateValue) return;
                _playingStateValue = value;
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
            playingState = PlayingStatuses.NotStarted;
            endTime = -1;
        }
    }
}