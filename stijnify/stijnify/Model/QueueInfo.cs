using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Model
{
    public class QueueInfo
    {
        public List<SongInfoModel> _StandardQueue { get; set; }
        public List<SongInfoModel> _CustomQueue { get; set; }
        public int _StandardSelectedSong { get; set; }
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
