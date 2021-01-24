using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace stijnify.Data.Interface
{
    public interface IPlayListRepository
    {
        /// <summary>
        /// Get all playlists
        /// </summary>
        /// <returns>List of all playlists</returns>
        List<PlayListModel> GetPlayLists();

        /// <summary>
        /// Add a new playlist
        /// </summary>
        /// <param name="playlist"></param>
        void AddPlayList(PlayListModel playlist);

        /// <summary>
        /// Delete a playlist
        /// </summary>
        /// <param name="playlist"></param>
        void DeletePlayList(PlayListModel playlist);

        /// <summary>
        /// Count all songs in a playlist
        /// </summary>
        /// <param name="playlist"></param>
        /// <returns>Number of songs</returns>
        int CountSongsPlayList(PlayListModel playlist);
    }
}
