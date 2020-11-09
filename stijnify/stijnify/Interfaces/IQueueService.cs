using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Interfaces
{
    interface IQueueService
    {
        SongInfoModel GetQueueItem();

        SongInfoModel NextQueueItem();

        SongInfoModel PreviousQueueItem();

        bool HasNext();

        bool HasPrevious();

        void ForcePlayItem(SongInfoModel song, List<SongInfoModel> queue);
    }
}
