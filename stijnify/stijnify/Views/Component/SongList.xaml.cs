using MediaManager.Playback;
using stijnify.Data;
using stijnify.Enum;
using stijnify.Model;
using stijnify.Services;
using stijnify.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stijnify.Views.Component
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongList : ContentView
    {
        #region Init variables

        /// <summary>
        /// Viewmodel of this component
        /// </summary>
        SongListModel ViewModel;

        /// <summary>
        /// The function to retrieve the songs
        /// </summary>
        Func<string, ObservableCollection<SongInfoModel>> _songListRetrieveMethod;

        /// <summary>
        /// the type of songlist
        /// </summary>
        SongListType _songListType;

        /// <summary>
        /// The connection to the database and his action
        /// </summary>
        SongRepository _database { get; set; }

        #endregion

        #region Initialisation
        public SongList()
        {
            InitializeComponent();

            ViewModel = new SongListModel();
            _database = new SongRepository();

            Constants.MediaPlayer.StateChanged += MediaPlayer_StateChanged;
        }

        /// <summary>
        /// Init the songlist
        /// </summary>
        /// <param name="songListRetrieveMethod"></param>
        /// <param name="songListType"></param>
        public void InitSongList(Func<string, ObservableCollection<SongInfoModel>> songListRetrieveMethod, SongListType songListType)
        {
            _songListRetrieveMethod = songListRetrieveMethod;

            _songListType = songListType;

            LoadSongs();
            SelectCurrentPlayingSong();
        }

        /// <summary>
        /// Load the songs in the ListView
        /// </summary>
        private void LoadSongs(string searchText = null)
        {
            var songList = _songListRetrieveMethod(searchText);
            
            if (songList == null) return; //Don't add songs to item list if there are not songs found

            ViewModel.SongList = songList;
            songListView.ItemsSource = ViewModel.SongList;
        }

        /// <summary>
        /// Select the song that is currently playing
        /// </summary>
        private void SelectCurrentPlayingSong()
        {
            MainPage mainPage = Application.Current.MainPage as MainPage;
            
            if (mainPage == null) return;

            var queue = mainPage.QueueService;
            var songItem = queue.GetQueueItem();
            if (songItem != null)
            {
                var foundSongItem = ViewModel.SongList.Where(e => e.Name == songItem.Name).FirstOrDefault();

                songListView.SelectedItem = foundSongItem;
            }
        }
        #endregion

        #region Events

        /// <summary>
        /// Event when character is entered in Search bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadSongs(e.NewTextValue);
        }

        /// <summary>
        /// When a song is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void songListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var songInfo = (SongInfoModel)e.Item;
            var listItemSource = ((ListView)sender).ItemsSource;
            var fullSongList = listItemSource.Cast<SongInfoModel>().ToList();

            MediaPlayerService.SelectSong(songInfo, fullSongList);
        }

        /// <summary>
        /// When the state of the mediaplayer changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayer_StateChanged(object sender, StateChangedEventArgs e)
        {
            SelectCurrentPlayingSong();
        }

        /// <summary>
        /// Renew song list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void songListView_Refreshing(object sender, EventArgs e)
        {
            LoadSongs();
            songListView.EndRefresh();
        }
        #endregion

        #region Song Options

        /// <summary>
        /// Event when options for song are clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SongOptions_Clicked(object sender, EventArgs e)
        {
            SongInfoModel song = (SongInfoModel)((ImageButton)sender).CommandParameter; //Retrieve the chosen song

            List<string> options = new List<string>() {
                "Delete file",
                "Song info",
                "Add To Queue",
                "Add to Playlist",
            };

            //Check which page the list is in and add aditional items
            if(_songListType == SongListType.Playlist)
            {
                options.Add("Delete from PlayList");
            }

            var response = await App.Current.MainPage.DisplayActionSheet("Song Options", "Cancel", null, options.ToArray());

            if (response == null) return;

            RunSelectedOption(response, song);
        }

        /// <summary>
        /// Run trough all options and check which one is selected
        /// </summary>
        /// <param name="optionSelected"></param>
        /// <param name="song"></param>
        private void RunSelectedOption(string optionSelected, SongInfoModel song)
        {
            if (optionSelected.ToLower() == "delete file")
            {
                DeleteSong(song.Path);
            }
            else if (optionSelected.ToLower() == "add to queue")
            {
                AddToQueue(song);
            }
            else if (optionSelected.ToLower() == "add to playlist")
            {
                StoreSongToPlayList(song);
            }
            else if (optionSelected.ToLower() == "delete from playlist")
            {
                DeleteFromPlaylist(song);
            }
            else if (optionSelected.ToLower() == "song info")
            {
                SongInfo(song);
            }
        }

        #endregion

        #region SongOptionActions

        private async void StoreSongToPlayList(SongInfoModel song)
        {
            var playlists = new PlayListRepository().GetPlayLists();
            List<string> playListChooseList = new List<string>();

            foreach (var playlist in playlists)
            {
                playListChooseList.Add(playlist.Title);
            }

            var result = await App.Current.MainPage.DisplayActionSheet("Choose a playlist", "Cancel", "Ok", playListChooseList.ToArray());

            if (result == "cancel") return;

            var chosenPlaylist = playlists.Where(playlist => playlist.Title == result).FirstOrDefault();

            var database = new SongRepository();
            song.PlayListId = chosenPlaylist.Id;
            database.AddSongToPlayList(song);
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
            bool deleteResponse = await App.Current.MainPage.DisplayAlert("Delete song", "Are you sure you want to delete this song?", "Delete", "Cancel");
            if (deleteResponse)
                deletedStatus = FileService.DeleteFile(path);

            if (!deletedStatus)
                return;

            //if (!String.IsNullOrEmpty(songSearchBar.Text))
            //    searchText = songSearchBar.Text;

            //GetAllFiles(searchText);
        }

        /// <summary>
        /// Add an item to the queue
        /// </summary>
        /// <param name="song"></param>
        private void AddToQueue(SongInfoModel song)
        {
            var queue = ((MainPage)Application.Current.MainPage).QueueService;
            queue.StoreQueueItem(song);
        }

        /// <summary>
        /// All actions needed for deleting a song
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async void DeleteFromPlaylist(SongInfoModel song)
        {
            bool deleteResponse = await App.Current.MainPage.DisplayAlert("Delete song from playlist", "Are you sure you want to delete this song from your playlist?", "Delete", "Cancel");

            if (deleteResponse)
                _database.RemoveSongFromPlayList(song);

            LoadSongs();
        }

        /// <summary>
        /// Get information about the song
        /// </summary>
        /// <param name="song"></param>
        private async void SongInfo(SongInfoModel song)
        {
            string songInfoMessage = "";
            // List of all the lines in playlist info popup
            string[] songInfo = {
                "Title: " + song.Name,
                "Path: " + song.Path,
            };

            // Loop trough all the lines and put them in one string to show as message in DisplayAlert
            foreach (string infoItem in songInfo)
            {
                songInfoMessage += infoItem + "\n";
            }

            //var test = database.CountSongsPlayList(playlist);
            await App.Current.MainPage.DisplayAlert("Playlist info", songInfoMessage, "Close");
        }

        #endregion
    }
}