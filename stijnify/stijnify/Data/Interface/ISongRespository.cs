using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Data.Interface
{
    interface ISongRespository
    {
        List<SongInfoModel> GetSongsOfPlayList(PlayListModel playlist);

        void AddSongToPlayList(SongInfoModel song);

        void RemoveSongFromPlayList(SongInfoModel song);
    }
}
