using DynamicData.Diagnostics;
using Plugin.Permissions.Abstractions;
using stijnify.Interfaces;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace stijnify.Services
{
    public class FileService
    {
        /// <summary>
        /// Retrieve all songs in the list of folders
        /// </summary>
        /// <param name="folderList"></param>
        /// <returns>ObservableCollection of songs</returns>
        public static ObservableCollection<SongInfoModel> GetAllSongs(List<string> folderList)
        {
            ObservableCollection<SongInfoModel> allSongs = new ObservableCollection<SongInfoModel>();

            //Go trough every folder and get all files
            foreach (string folder in folderList)
            {
                //Retrieve all files with the Extension .mp3
                var allFiles = Directory.GetFiles(folder).Where(file => Path.GetExtension(file) == ".mp3");

                //Add every item in the list of songs
                foreach (string file in allFiles)
                    allSongs.Add(new SongInfoModel()
                    {
                        Name = Path.GetFileNameWithoutExtension(file),
                        Path = file
                    });
            }

            return allSongs;
        }

        /// <summary>
        /// Delete  a file
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Status of deleted file</returns>
        public static bool DeleteFile(string path)
        {
            if (!Directory.Exists(path))
                return false;

            Directory.Delete(path);
            return true;
        }
    }
}
