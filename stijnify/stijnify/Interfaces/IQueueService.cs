using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Interfaces
{
    interface IQueueService
    {
        /// <summary>
        /// Get the current queue item
        /// </summary>
        /// <returns>Current queue item</returns>
        SongInfoModel GetQueueItem();

        /// <summary>
        /// Get the next queue item
        /// </summary>
        /// <returns>Next queue item</returns>
        SongInfoModel NextQueueItem();

        /// <summary>
        /// Get the previous queue item
        /// </summary>
        /// <returns>previous queue item</returns>
        SongInfoModel PreviousQueueItem();

        /// <summary>
        /// Has the queue a next song
        /// </summary>
        /// <returns>boolean if there is a next</returns>
        bool HasNext();

        /// <summary>
        /// Has the queue a previous song
        /// </summary>
        /// <returns>boolean if there is a previous</returns>
        bool HasPrevious();

        /// <summary>
        /// Force a song to play
        /// </summary>
        /// <param name="song"></param>
        /// <param name="queue"></param>
        void ForcePlayItem(SongInfoModel song, List<SongInfoModel> queue);

        /// <summary>
        /// Remove an item from the queue
        /// </summary>
        /// <param name="removeIndex"></param>
        void RemoveCustomQueueItem(SongInfoModel removeIndex);

        /// <summary>
        /// When custom queue ended reset it
        /// </summary>
        void CustomQueueEnded();

        /// <summary>
        /// Store a queue item
        /// </summary>
        /// <param name="song"></param>
        void StoreQueueItem(SongInfoModel song);
    }
}
