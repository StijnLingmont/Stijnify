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
        private PlayListModel playlist { get; set; }
        public PlayListShowPage(PlayListModel c_playlist)
        {
            InitializeComponent();

            playlist = c_playlist;

            selectedPlayListTitle.Text = playlist.Title;

            var database = new SongRepository();
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
    }
}