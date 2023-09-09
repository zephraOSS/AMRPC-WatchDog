using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AMRPC_WatchDog_Desktop
{
    public class Payload
    {
        private string _playingStatusValue;
        private double _endTimeValue = -1;

        public event PropertyChangedEventHandler PropertyChanged;

        public static class PlayingStatuses
        {
            public const string Playing = "playing";
            public const string NotStarted = "not_started";
            public const string Paused = "paused";
        }

        public string Title { get; set; }
        public string Album { get; set; }
        public string Artist { get; set; }
        public string ThumbnailPath { get; set; }

        public string PlayingStatus
        {
            get => _playingStatusValue;
            set
            {
                if (value == _playingStatusValue) return;
                _playingStatusValue = value;
                NotifyPropertyChanged();
            }
        }

        public double EndTime
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
            Title = null;
            Artist = null;
            Album = null;
            ThumbnailPath = null;
            PlayingStatus = PlayingStatuses.NotStarted;
            EndTime = -1;
        }
    }
}