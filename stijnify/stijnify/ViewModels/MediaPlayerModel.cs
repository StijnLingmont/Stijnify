using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.ViewModels
{
    class MediaPlayerModel : ReactiveObject
    {
        string _playPause;
        int _maxSecondsSong;
        int _progressSecondsSong;
        string _maxLengthSong;
        string _progressLengthSong;

        public string PlayPause
        {
            get
            {
                return _playPause;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _playPause, value);
            }
        }

        public string ProgressLengthSong
        {
            get
            {
                return _progressLengthSong;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _progressLengthSong, value);
            }
        }

        public int MaxSecondsSong
        {
            get
            {
                return _maxSecondsSong;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _maxSecondsSong, value);
            }
        }

        public int ProgressSecondsSong
        {
            get
            {
                return _progressSecondsSong;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _progressSecondsSong, value);
            }
        }

        public string MaxLengthSong
        {
            get
            {
                return _maxLengthSong;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _maxLengthSong, value);
            }
        }

        public MediaPlayerModel()
        {
            _playPause = "play";
            _maxLengthSong = "00:00";
            _maxSecondsSong = 0;
            _progressLengthSong = "00:00";
            _progressSecondsSong = 0;
        }
    }
}
