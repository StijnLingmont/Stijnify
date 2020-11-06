using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.ViewModels
{
    class MediaPlayerModel : ReactiveObject
    {
        string _playPause;

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

        public MediaPlayerModel()
        {
            _playPause = "play";
        }
    }
}
