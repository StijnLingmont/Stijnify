using MediaManager.Playback;
using stijnify.Data;
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
        SongListModel ViewModel;

        public SongList()
        {
            InitializeComponent();

            ViewModel = new SongListModel();


            Constants.MediaPlayer.StateChanged += MediaPlayer_StateChanged;
        }

        public void SetSongs(ObservableCollection<SongInfoModel> songList)
        {
            Console.WriteLine(BindingContext);
            ViewModel.SongList = songList;
            songListView.ItemsSource = ViewModel.SongList;
        }

        private void songListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var songInfo = (SongInfoModel)e.Item;
            var listItemSource = ((ListView)sender).ItemsSource;
            var fullSongList = listItemSource.Cast<SongInfoModel>().ToList();

            MediaPlayerService.SelectSong(songInfo, fullSongList);
        }

        private void MediaPlayer_StateChanged(object sender, StateChangedEventArgs e)
        {
            var queue = ((MainPage)Application.Current.MainPage).QueueService;
            var songItem = queue.GetQueueItem();
            var foundSongItem = ViewModel.SongList.Where(e => e.Name == songItem.Name).FirstOrDefault();

            songListView.SelectedItem = foundSongItem;
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
            var response = await App.Current.MainPage.DisplayActionSheet("Song Options", "Cancel", null,"Delete file" ,"Add To PlayList", "Add To Queue");

            if (response.ToLower() == "delete file")
            {
                DeleteSong(songInfo.Path);
            }
            else if (response.ToLower() == "add to queue")
            {
                AddToQueue(songInfo);
            }
            else if (response.ToLower() == "add to playlist")
            {
                StoreSongToPlayList(songInfo);
            }
        }

        private async void StoreSongToPlayList(SongInfoModel song)
        {
            var playlists = new PlayListRepository().GetPlayLists();
            List<string> playListChooseList = new List<string>();

            foreach(var playlist in playlists)
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

        private void AddToQueue(SongInfoModel song)
        {
            var queue = ((MainPage)Application.Current.MainPage).QueueService;
            queue.StoreQueueItem(song);
        }
        #endregion
    }
}