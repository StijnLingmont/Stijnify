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

namespace stijnify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        #region Initialise Variables

        HomePageModel ViewModel;

        #endregion

        public HomePage()
        {
            InitializeComponent();

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
                List<string> folderList;

                //Get all selected folders
                var folders = Preferences.Get("folders", null);

                if (folders == null)
                    return;

                //Convert string in List of strings
                folderList = new List<string>(folders.Split(','));

                //Retrieve all songs from the folders
                allSongs = FileService.GetAllSongs(folderList);

                songListView.SetSongs(allSongs);

                //Check if send full list or just searched List
                if (String.IsNullOrWhiteSpace(searchText))
                    ViewModel.SongList = allSongs;
                else
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

        /// <summary>
        /// Event when character is entered in Search bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetAllFiles(e.NewTextValue);
        }
    }
}