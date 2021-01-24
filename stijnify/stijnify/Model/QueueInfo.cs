using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Model
{
    public class QueueInfo
    {
        /// <summary>
        /// The list with the standard queue where the full list is in.
        /// </summary>
        public List<SongInfoModel> _StandardQueue { get; set; }

        /// <summary>
        /// The list with the custom selected queue items from the user
        /// </summary>
        public List<SongInfoModel> _CustomQueue { get; set; }

        /// <summary>
        /// The current selected song from the standard queue
        /// </summary>
        public int _StandardSelectedSong { get; set; }

        /// <summary>
        /// The current selected song from the custom queue
        /// </summary>
        public int _CustomSelectedSong { get; set; }

        public QueueInfo()
        {
            _StandardQueue = new List<SongInfoModel>();
            _CustomQueue = new List<SongInfoModel>();
            _CustomSelectedSong = -1;
            _StandardSelectedSong = -1;
        }
    }
}
