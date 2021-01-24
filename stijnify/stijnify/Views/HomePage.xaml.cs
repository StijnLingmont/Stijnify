using Xamarin.Essentials;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using stijnify.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using stijnify.Services;
using stijnify.ViewModels;
using stijnify.Model;
using MediaManager.Playback;
using stijnify.Data;
using DynamicData;
using stijnify.Views.Component;
using stijnify.Enum;

namespace stijnify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            songListView.InitSongList(GetAllFiles, SongListType.AllSongs);
        }

        /// <summary>
        /// Get All Files from the folders chosen in the Settings
        /// </summary>
        ObservableCollection<SongInfoModel> GetAllFiles(string searchText = null)
        {
            try
            {
                ObservableCollection<SongInfoModel> allSongs = new ObservableCollection<SongInfoModel>();
                List<string> folderList;

                //Get all selected folders
                var folders = Preferences.Get("folders", null);

                if (folders == null)
                    return null;

                //Convert string in List of strings
                folderList = new List<string>(folders.Split(','));

                //Retrieve all songs from the folders
                allSongs = FileService.GetAllSongs(folderList);

                //Check if send full list or just searched List
                if (String.IsNullOrWhiteSpace(searchText))
                    return allSongs;
                else
                    return new ObservableCollection<SongInfoModel>(allSongs.Where(song => song.Name.ToLower().Contains(searchText.ToLower())));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            };
        }
    }
}