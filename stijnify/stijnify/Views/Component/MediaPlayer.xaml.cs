using Autofac;
using MediaManager;
using MediaManager.Library;
using MediaManager.Playback;
using MediaManager.Player;
using stijnify.Interfaces;
using stijnify.Model;
using stijnify.Services;
using stijnify.ViewModels;
using stijnify.Views.Temp_Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PositionChangedEventArgs = MediaManager.Playback.PositionChangedEventArgs;

namespace stijnify.Views.Component
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPlayer : ContentView
    {
        #region Init variables

        /// <summary>
        /// The viewmodel of this component
        /// </summary>
        MediaPlayerModel ViewModel { get; set; }

        /// <summary>
        /// The mediaplayer service to manage the mediaplayer
        /// </summary>
        IMediaPlayerService _mediaPlayerService;

        /// <summary>
        /// Status if the progressbar is dragging
        /// </summary>
        bool _progressBarIsDragging = false;

        /// <summary>
        /// The progress timer to check progress
        /// </summary>
        Timer _progressTimer = null;

        #endregion

        #region Init component

        public MediaPlayer()
        {
            InitializeComponent();

            _mediaPlayerService = Container.ContainerInstance.Resolve<IMediaPlayerService>();

            BindingContext = ViewModel = new MediaPlayerModel();

            InitEvents();
        }

        /// <summary>
        /// Init all events
        /// </summary>
        void InitEvents()
        {
            Constants.MediaPlayer.StateChanged += StateChanged;
            Constants.MediaPlayer.MediaItemFinished += MediaPlayer_MediaItemFinished;
        }

        #endregion

        /// <summary>
        /// Event for Reseting base progress
        /// </summary>
        void ResetBaseProgress()
        {
            ViewModel.ProgressLengthSong = "00:00";
            ViewModel.ProgressSecondsSong = 0;
        }

        #region MediaPlayer Events

        /// <summary>
        /// When the song is done
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayer_MediaItemFinished(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            ResetBaseProgress();
        }

        /// <summary>
        /// Update the position of the song
        /// </summary>
        private void PositionUpdate()
        {
            //Calculate maximum of the song
            var song = Constants.MediaPlayer.Queue.Current;
            var position = Constants.MediaPlayer.Position;

            if (song == null) return;

            int maxTotalSeconds = (int)song.Duration.TotalSeconds;
            string maxMinutes = Math.Floor(song.Duration.TotalMinutes).ToString("00");
            string maxSeconds = song.Duration.Seconds.ToString("00");

            //Max song progress
            ViewModel.MaxLengthSong = $"{maxMinutes}:{maxSeconds}";
            ViewModel.MaxSecondsSong = maxTotalSeconds;

            //Update progress if progress bar is not dragging
            if (!_progressBarIsDragging)
                {
                    int totalSeconds = (int)position.TotalSeconds;
                    ViewModel.ProgressSecondsSong = totalSeconds;

                    string minutes = Math.Floor(position.TotalMinutes).ToString("00");
                    string seconds = position.Seconds.ToString("00");

                    ViewModel.ProgressLengthSong = $"{minutes}:{seconds}";
            }
        }

        /// <summary>
        /// Event for Chaning state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void StateChanged(object sender, StateChangedEventArgs e)
        {
            var state = e.State;
            _progressTimer = null;
            if (state == MediaManager.Player.MediaPlayerState.Playing)
            {
                ViewModel.PlayPause = "pause";
                var startTimeSpan = TimeSpan.Zero;
                var periodTimeSpan = TimeSpan.FromMilliseconds(500);

                if(_progressTimer == null)
                {
                    _progressTimer = new Timer((e) =>
                    {
                        PositionUpdate();
                    }, null, startTimeSpan, periodTimeSpan);
                }
            }
            else if (state == MediaPlayerState.Paused || state == MediaPlayerState.Stopped)
            {
                //Remove the timer if the song is paused/stopped
                if(_progressTimer != null)
                    _progressTimer.Dispose();

                ViewModel.PlayPause = "play";
            }
        }

        #endregion

        #region View Events

        /// <summary>
        /// Event for pausing or resuming song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PauseResume_Clicked(object sender, EventArgs e)
        {
            _mediaPlayerService.PlayPauseToggle();
        }

        /// <summary>
        /// Event for changing progress song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongProgress_DragCompleted(object sender, EventArgs e)
        {
            var slider = (Slider)sender;

            double value = slider.Value;

            TimeSpan newTimeSong = TimeSpan.FromSeconds(value);

            _mediaPlayerService.ChangePosition(newTimeSong);

            _progressBarIsDragging = false;
        }

        /// <summary>
        /// Event for when Slider is changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slider_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (ViewModel == null)
                return;

            if(_progressBarIsDragging)
            {
                var slider = (Slider)sender;

                TimeSpan newTimeSong = TimeSpan.FromSeconds(slider.Value);
                string minutes = Math.Floor(newTimeSong.TotalMinutes).ToString("00");
                string seconds = newTimeSong.Seconds.ToString("00");

                ViewModel.ProgressLengthSong = $"{minutes}:{seconds}";
            }
        }

        /// <summary>
        /// Event for when drag started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progresSlider_DragStarted(object sender, EventArgs e)
        {
            _progressBarIsDragging = true;
        }

        /// <summary>
        /// Event when clicking previous button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Previous_Clicked(object sender, EventArgs e)
        {
            ResetBaseProgress();
            _mediaPlayerService.Previous();
        }

        /// <summary>
        /// Event when clicking next button
        /// </summary>s
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_Clicked(object sender, EventArgs e)
        {
            ResetBaseProgress();
            _mediaPlayerService.Next();
        }

        /// <summary>
        /// Event when click queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Queue_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new QueueView());
        }

        #endregion
    }
}