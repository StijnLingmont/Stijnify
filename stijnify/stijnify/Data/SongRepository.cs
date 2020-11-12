using SQLite;
using stijnify.Data.Interface;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace stijnify.Data
{
    class SongRepository : ISongRespository
    {
        private SQLiteConnection _connection;

        public SongRepository()
        {
            _connection = DBConnection.Initialise();

            _connection.CreateTable<SongInfoModel>();
        }

        public void AddSongToPlayList(SongInfoModel playlist)
        {
            _connection.Insert(playlist);
        }

        public List<SongInfoModel> GetSongsOfPlayList(PlayListModel playlist)
        {
            var result = _connection.Query<SongInfoModel>($"SELECT * FROM SongInfoModel WHERE PlayListId = {playlist.Id}").ToList();
            return result;
        }

        public void RemoveSongFromPlayList(SongInfoModel playlist)
        {
            throw new NotImplementedException();
        }
    }
}
