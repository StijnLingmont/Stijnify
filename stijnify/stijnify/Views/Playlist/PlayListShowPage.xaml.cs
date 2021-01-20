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
    }
}