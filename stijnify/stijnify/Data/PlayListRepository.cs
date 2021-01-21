using SQLite;
using stijnify.Data.Interface;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace stijnify.Data
{
    class PlayListRepository : IPlayListRepository
    {
        private SQLiteConnection _connection;

        public PlayListRepository()
        {
            _connection = DBConnection.Initialise();

            _connection.CreateTable<PlayListModel>();
        }

        public void AddPlayList(PlayListModel playlist)
        {
            _connection.Query<PlayListModel>($"INSERT INTO PlayListModel(title) VALUES('{playlist.Title}')");
        }

        public List<PlayListModel> GetPlayLists()
        {
            try
            {
                return _connection.Table<PlayListModel>().ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void DeletePlayList(PlayListModel playlist)
        {
            _connection.Delete(playlist);
        }

        public int CountSongsPlayList(PlayListModel playlist)
        {
            var result = _connection.Query<SongInfoModel>($"SELECT * FROM SongInfoModel WHERE PlayListId = {playlist.Id}").ToList();
            return result.Count;
        }
    }
}
