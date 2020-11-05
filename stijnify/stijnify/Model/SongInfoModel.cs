using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Model
{
    class SongInfoModel
    {
        /// <summary>
        /// Name of the song
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Path of the song
        /// </summary>
        public string Path { get; set; }

        public SongInfoModel(string c_name, string c_path)
        {
            Name = c_name;
            Path = c_path;
        }
    }
}
