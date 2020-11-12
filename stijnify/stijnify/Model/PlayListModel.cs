using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Model
{
    public class PlayListModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        [Unique]
        public string Title { get; set; }
    }
}
