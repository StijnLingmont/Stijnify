using stijnify.Interfaces;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Xamarin.Forms;

namespace stijnify.Services
{
    public class QueueService : IQueueService
    {
        public QueueInfo _queue { get; set; }
        public QueueService()
        {
            _queue = new QueueInfo();
        }

        public SongInfoModel GetQueueItem()
        {
            int queueItemIndex;
            bool getCustomItem = _queue._CustomQueue.Count > 0;

            //Check if there is added custom queue items
            if (getCustomItem)
            {
                queueItemIndex = _queue._CustomSelectedSong;
                return _queue._CustomQueue[queueItemIndex];
            }
            else
            {
                queueItemIndex = _queue._StandardSelectedSong;
                return _queue._StandardQueue[queueItemIndex];
            }

        }

        public void RemoveCustomQueueItem(SongInfoModel song)
        {
            _queue._CustomQueue.Remove(song);
        }

        public SongInfoModel NextQueueItem()
        {
            CustomQueueEnded();

            var hasCustomQueue = _queue._CustomQueue.Count > 0;

            if (!HasNext())
                return null;

            //Check if you need to go to the next song of the standard queue or the custom queue
            if (hasCustomQueue)
            {
                //Check if you need to start the custom queue or go trough with it
                if (_queue._CustomSelectedSong == -1)
                    _queue._CustomSelectedSong = 0;
                else
                    _queue._CustomSelectedSong++;
            }
            else
            {
                _queue._StandardSelectedSong++;
            }

            return GetQueueItem();
        }

        public void CustomQueueEnded()
        {
            //Check if custom queue is finished
            if (_queue._CustomQueue.Count > 0 && _queue._CustomSelectedSong + 1 >= _queue._CustomQueue.Count)
            {
                _queue._CustomQueue.Clear();
                _queue._CustomSelectedSong = -1;
            }
        }

        public SongInfoModel PreviousQueueItem()
        {
            if (!HasPrevious())
                return null;

            if (_queue._CustomQueue.Count > 0)
                _queue._CustomSelectedSong--;
            else
                _queue._StandardSelectedSong--;

            return GetQueueItem();
        }

        public void StoreQueueItem(SongInfoModel song)
        {
            _queue._CustomQueue.Add(song);
        }

        public void ForcePlayItem(SongInfoModel song, List<SongInfoModel> queue)
        {
            if(_queue._CustomQueue.Count > 0)
            {
                _queue._CustomQueue[_queue._CustomSelectedSong] = song;
            } 
            else
            {
                _queue._StandardQueue = queue;
                _queue._StandardSelectedSong = queue.IndexOf(song);
            }
        }

        public bool HasPrevious()
        {
            if (_queue._CustomQueue.Count > 0)
                return _queue._CustomSelectedSong > 0;
            else
                return _queue._StandardSelectedSong > 0;
        }

        public bool HasNext()
        {
            if (_queue._CustomQueue.Count > 0)
                return _queue._CustomQueue.Count > _queue._CustomSelectedSong + 1;
            else
                return _queue._StandardQueue.Count > _queue._StandardSelectedSong + 1;
        }
    }
}
