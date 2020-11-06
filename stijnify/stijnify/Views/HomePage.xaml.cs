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

namespace stijnify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        #region Initialise Variables

        /// <summary>
        /// Root Folder of the device
        /// </summary>
        string _RootFolder;

        HomePageModel ViewModel;

        #endregion

        public HomePage()
        {
            InitializeComponent();

            _RootFolder = DirectoryService.GetRootDirectory().Result;

            BindingContext = ViewModel = new HomePageModel();

            GetAllFiles();
        }

        /// <summary>
        /// Get All Files from the folders chosen in the Settings
        /// </summary>
        void GetAllFiles(string searchText = null)
        {
            try
            {
                ObservableCollection<SongInfoModel> allSongs = new ObservableCollection<SongInfoModel>();
                ObservableCollection<string> folderList;

                //Get all selected folders
                var folders = Preferences.Get("folders", null);

                if (folders == null)
                    return;

                folderList = new ObservableCollection<string>(folders.Split(','));

                //Go trough every folder and get all files
                foreach(string folder in folderList)
                {
                    //Retrieve all files with the Extension .mp3
                    var allFiles = Directory.GetFiles(folder).Where(file => Path.GetExtension(file) == ".mp3");

                    //Add every item in the list of songs
                    foreach (string file in allFiles)
                        allSongs.Add(new SongInfoModel(Path.GetFileNameWithoutExtension(file), file));
                }

                //Return the full list when user is not searching
                if (String.IsNullOrWhiteSpace(searchText))
                    ViewModel.SongList = allSongs;

                //Retrieve the searched results
                ViewModel.SongList = new ObservableCollection<SongInfoModel>(allSongs.Where(song => song.Name.ToLower().Contains(searchText.ToLower())));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };
        }

        /// <summary>
        /// Event for refresh song list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void songListView_Refreshing(object sender, EventArgs e)
        {
            var listView = (ListView)sender;

            GetAllFiles();

            listView.EndRefresh();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetAllFiles(e.NewTextValue);
        }

        private async void SongOptions_Clicked(object sender, EventArgs e)
        {
            SongInfoModel songInfo = (SongInfoModel)((ImageButton)sender).CommandParameter;
            var response = await DisplayActionSheet("Song Options", "Cancel", null,"Delete file" ,"Play", "Add To Queue");
            string searchText = null;

            if (response.ToLower() == "delete file")
                await DeleteFile(songInfo.Path);

            if (!String.IsNullOrEmpty(songSearchBar.Text))
                searchText = songSearchBar.Text;

            GetAllFiles(searchText);
        }

        /// <summary>
        /// Delete a song
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task<bool> DeleteFile(string path)
        {
            bool response = await DisplayAlert("Delete song", "Are you sure you want to delete this song?", "Delete", "Cancel");
            if(response)
                File.Delete(path);

            return true;
        }
    }
}