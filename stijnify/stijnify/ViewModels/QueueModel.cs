using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace stijnify.Model
{
    public class QueueModel : ReactiveObject
    {
        ObservableCollection<SongInfoModel> _customQueue;

        public ObservableCollection<SongInfoModel> CustomQueue
        {
            get
            {
                return _customQueue;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _customQueue, value);
            }
        }
    }
}
