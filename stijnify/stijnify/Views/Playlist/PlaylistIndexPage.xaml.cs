using stijnify.Data;
using stijnify.Interfaces;
using stijnify.Model;
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
    public partial class PlaylistIndexPage : ContentPage
    {
        PlayListRepository database;
        public PlaylistIndexPage()
        {
            InitializeComponent();

            database = new PlayListRepository();
            InitPlayLists();
        }

        /// <summary>
        /// Initiliase Playlists
        /// </summary>
        void InitPlayLists()
        {
            database = new PlayListRepository();
            var playlists = database.GetPlayLists();
            playlistList.ItemsSource = new ObservableCollection<PlayListModel>(playlists);
        }

        /// <summary>
        /// Event for adding Playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddPlayList_Clicked(object sender, EventArgs e)
        {
            //Get playlist name from user
            string playListName = await DisplayPromptAsync("Creating PlayList", "PlayList Title:");

            if (playListName == null) return;

            //Create PlayList data
            var playlist = new PlayListModel()
            {
                Title = playListName
            };

            //Add Playlist to Database
            database.AddPlayList(playlist);

            RetrievePlayList();
        }

        /// <summary>
        /// Retrieve the playlist
        /// </summary>
        private void RetrievePlayList()
        {
            //Get new list of playlist
            var playlists = database.GetPlayLists();
            playlistList.ItemsSource = new ObservableCollection<PlayListModel>(playlists);
        }

        /// <summary>
        /// Event clicking options for playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void PlayListOptions_Clicked(object sender, EventArgs e)
        {
            var playlist = (PlayListModel)((ImageButton)sender).BindingContext;

            var chosenOption = await DisplayActionSheet("Playlist options", "Cancel", null, "Delete playlist");

            if (chosenOption.ToLower() == "delete playlist")
                database.DeletePlayList(playlist);

            RetrievePlayList();
        }

        private void PlayListItem_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var playlist = e.Item as PlayListModel;

            Navigation.PushAsync(new PlayListShowPage(playlist));
        }
    }
}