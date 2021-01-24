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

        /// <summary>
        /// Connection to the database and all his actions
        /// </summary>
        SongRepository database { get; set; }

        /// <summary>
        /// The current selected playlist
        /// </summary>
        private PlayListModel playlist { get; set; }

        #endregion

        public PlayListShowPage(PlayListModel c_playlist)
        {
            InitializeComponent();

            // Select the current playlist
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
            var songsOfPlaylist = new ObservableCollection<SongInfoModel>(database.GetSongsOfPlayList(playlist));

            //Check if send full list or just searched List
            if (String.IsNullOrWhiteSpace(searchText))
                return songsOfPlaylist;
            else
                return new ObservableCollection<SongInfoModel>(songsOfPlaylist.Where(song => song.Name.ToLower().Contains(searchText.ToLower())));
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