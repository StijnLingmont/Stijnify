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

            Constants.MediaPlayer.StateChanged += MediaPlayer_StateChanged;

            GetAllFiles();
        }

        private void MediaPlayer_StateChanged(object sender, StateChangedEventArgs e)
        {
            var queue = ((MainPage)Application.Current.MainPage).QueueService;
            songListView.SelectedItem = queue.GetQueueItem();
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

        #region Song Options

        /// <summary>
        /// Event when options for song are clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SongOptions_Clicked(object sender, EventArgs e)
        {
            SongInfoModel songInfo = (SongInfoModel)((ImageButton)sender).CommandParameter;
            var response = await DisplayActionSheet("Song Options", "Cancel", null,"Delete file" ,"Play", "Add To Queue");

            if (response.ToLower() == "delete file")
            {
                DeleteSong(songInfo.Path);
            }
            else if (response.ToLower() == "add to queue")
            {
                AddToQueue(songInfo);
            }
        }

        /// <summary>
        /// All actions needed for deleting a song
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async void DeleteSong(string path)
        {
            bool deletedStatus = false;
            string searchText = null;
            bool deleteResponse = await DisplayAlert("Delete song", "Are you sure you want to delete this song?", "Delete", "Cancel");
            if (deleteResponse)
                deletedStatus = FileService.DeleteFile(path);

            if (!deletedStatus)
                return;

            if (!String.IsNullOrEmpty(songSearchBar.Text))
                searchText = songSearchBar.Text;

            GetAllFiles(searchText);
        }

        private void AddToQueue(SongInfoModel song)
        {
            var queue = ((MainPage)Application.Current.MainPage).QueueService;
            queue.StoreQueueItem(song);
        }
        #endregion

        private void PlaySong_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var songInfo = (SongInfoModel)e.Item;
            var listItemSource = ((ListView)sender).ItemsSource;
            var fullSongList = listItemSource.Cast<SongInfoModel>().ToList();

            MediaPlayerService.SelectSong(songInfo, fullSongList);
        }
    }
}