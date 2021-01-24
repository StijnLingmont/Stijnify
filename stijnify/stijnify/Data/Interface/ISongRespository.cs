using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Data.Interface
{
    interface ISongRespository
    {
        /// <summary>
        /// Get all songs of a specefic playlist
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns>List of songs from that playlist</returns>
        List<SongInfoModel> GetSongsOfPlayList(PlayListModel playlist);

        /// <summary>
        /// Add a song to a playlist
        /// </summary>
        /// <param name="song"></param>
        void AddSongToPlayList(SongInfoModel song);

        /// <summary>
        /// Remove a song from a playlist
        /// </summary>
        /// <param name="song"></param>
        void RemoveSongFromPlayList(SongInfoModel song);
    }
}
