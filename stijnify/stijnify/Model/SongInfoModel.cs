using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Model
{
    public class SongInfoModel
    {
        /// <summary>
        /// The Id of the song
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Name of the song
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path of the song
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The Id of the playlist it is combined too
        /// </summary>
        public int PlayListId { get; set; }

        public SongInfoModel()
        {
        }
    }
}
