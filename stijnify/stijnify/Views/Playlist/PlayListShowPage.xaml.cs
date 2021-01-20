using stijnify.Data;
using stijnify.Enum;
using stijnify.Model;
using stijnify.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stijnify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class PlayListShowPage : ContentPage
    {
        #region Variable Ínit

        SongRepository database { get; set; }
        private PlayListModel playlist { get; set; }

        #endregion

        public PlayListShowPage(PlayListModel c_playlist)
        {
            InitializeComponent();

            playlist = c_playlist;

            selectedPlayListTitle.Text = playlist.Title;

            database = new SongRepository();

            playlistSongList.InitSongList(GetSongsFromPlayList, SongListType.Playlist);
        }

        /// <summary>
        /// Retrieve songs from playlist
        /// </summary>
        ObservableCollection<SongInfoModel> GetSongsFromPlayList(string searchText = null)
        {
            return new ObservableCollection<SongInfoModel>(database.GetSongsOfPlayList(playlist));
        }

        /// <summary>
        /// Event for back button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        /// <summary>
        /// Event for item tapped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playlistSongList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var songInfo = (SongInfoModel)e.Item;
            var listItemSource = ((ListView)sender).ItemsSource;
            var fullSongList = listItemSource.Cast<SongInfoModel>().ToList();

            MediaPlayerService.SelectSong(songInfo, fullSongList);
        }

        /// <summary>
        /// Event for options of a song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PlaylistItemOption_Clicked(object sender, EventArgs e)
        {
            SongInfoModel songInfo = (SongInfoModel)((ImageButton)sender).CommandParameter;
            var response = await DisplayActionSheet("Song Options", "Cancel", null, "Delete file", "Add To Queue");

            if (response.ToLower() == "delete file")
            {
                await DeleteFromPlaylist(songInfo);
            }
            else if (response.ToLower() == "add to queue")
            {

            }

            GetSongsFromPlayList();
        }

        /// <summary>
        /// All actions needed for deleting a song
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task DeleteFromPlaylist(SongInfoModel song)
        {
            bool deleteResponse = await DisplayAlert("Delete song from playlist", "Are you sure you want to delete this song from your playlist?", "Delete", "Cancel");
            
            if (deleteResponse)
                database.RemoveSongFromPlayList(song);
        }
    }
}