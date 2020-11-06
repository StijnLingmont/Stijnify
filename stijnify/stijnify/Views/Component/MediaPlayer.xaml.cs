using MediaManager.Playback;
using stijnify.Model;
using stijnify.Services;
using stijnify.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stijnify.Views.Component
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPlayer : ContentView
    {
        MediaPlayerModel ViewModel;
        MediaPlayerService _mediaPlayerService;
        public MediaPlayer()
        {
            InitializeComponent();

            _mediaPlayerService = new MediaPlayerService();

            BindingContext = ViewModel = new MediaPlayerModel();

            InitEvents();
        }

        void InitEvents()
        {
            Constants.MediaPlayer.StateChanged += StateChanged;
        }

        /// <summary>
        /// Event for Chaning state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StateChanged(object sender, StateChangedEventArgs e)
        {
            var state = e.State;
            if (state == MediaManager.Player.MediaPlayerState.Playing)
                ViewModel.PlayPause = "pause";
            else if (state == MediaManager.Player.MediaPlayerState.Paused)
                ViewModel.PlayPause = "play";
        }

        /// <summary>
        /// Event for pausing or resuming song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseResume_Clicked(object sender, EventArgs e)
        {
            _mediaPlayerService.PlayPauseToggle();
        }
    }
}