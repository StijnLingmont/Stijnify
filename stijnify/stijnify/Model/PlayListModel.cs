using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Model
{
    public class PlayListModel
    {
        /// <summary>
        /// The id of the playlist
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        /// <summary>
        /// The title of the playlist
        /// </summary>
        [Unique]
        public string Title { get; set; }
    }
}
