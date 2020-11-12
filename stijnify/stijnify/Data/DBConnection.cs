using SQLite;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace stijnify.Data
{
    class DBConnection
    {
        public static SQLiteConnection Initialise()
        {
            SQLiteConnection _connection;

            try
            {
                _connection = new SQLiteConnection(Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "playlists.db3"));
                return _connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
