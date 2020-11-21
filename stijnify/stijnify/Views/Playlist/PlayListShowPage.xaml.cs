using stijnify.Data;
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
        SongRepository database { get; set; }
        private PlayListModel playlist { get; set; }
        public PlayListShowPage(PlayListModel c_playlist)
        {
            InitializeComponent();

            playlist = c_playlist;

            selectedPlayListTitle.Text = playlist.Title;

            database = new SongRepository();
            GetSongsFromPlayList();
        }

        private void GetSongsFromPlayList()
        {
            ObservableCollection<SongInfoModel> songs = new ObservableCollection<SongInfoModel>(database.GetSongsOfPlayList(playlist));

            playlistSongList.ItemsSource = songs;
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void playlistSongList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var songInfo = (SongInfoModel)e.Item;
            var listItemSource = ((ListView)sender).ItemsSource;
            var fullSongList = listItemSource.Cast<SongInfoModel>().ToList();

            MediaPlayerService.SelectSong(songInfo, fullSongList);
        }

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