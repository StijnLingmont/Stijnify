﻿using Autofac;
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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PositionChangedEventArgs = MediaManager.Playback.PositionChangedEventArgs;

namespace stijnify.Views.Component
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaPlayer : ContentView
    {
        MediaPlayerModel ViewModel { get; set; }
        IMediaPlayerService _mediaPlayerService;
        bool _canProgress = true;

        public MediaPlayer()
        {
            InitializeComponent();

            _mediaPlayerService = Container.ContainerInstance.Resolve<IMediaPlayerService>();

            BindingContext = ViewModel = new MediaPlayerModel();

            InitEvents();
        }

        void InitEvents()
        {
            Constants.MediaPlayer.StateChanged += StateChanged;
            Constants.MediaPlayer.PositionChanged += CurrentSong_PositionChanged;
            Constants.MediaPlayer.MediaItemFinished += MediaPlayer_MediaItemFinished;
            Constants.MediaPlayer.BufferedChanged += MediaPlayer_BufferedChanged;
        }

        private void MediaPlayer_BufferedChanged(object sender, BufferedChangedEventArgs e)
        {
            Console.WriteLine(e.Buffered);
        }

        private void MediaPlayer_MediaItemFinished(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            ResetBaseProgress();
        }

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
        /// Event for when Song Position has been changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentSong_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            //Calculate maximum of the song
            var song = (MediaManagerBase)sender;
            int maxTotalSeconds = (int)song.Duration.TotalSeconds;
            string maxMinutes = Math.Floor(song.Duration.TotalMinutes).ToString("00");
            string maxSeconds = song.Duration.Seconds.ToString("00");

            //Max song progress
            ViewModel.MaxLengthSong = $"{maxMinutes}:{maxSeconds}";
            ViewModel.MaxSecondsSong = maxTotalSeconds;

            //Update progress if possible
            if (_canProgress)
            {
                int totalSeconds = (int)e.Position.TotalSeconds;
                ViewModel.ProgressSecondsSong = totalSeconds;
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
            if (state == MediaManager.Player.MediaPlayerState.Playing)
                ViewModel.PlayPause = "pause";
            else if (state == MediaPlayerState.Paused || state == MediaPlayerState.Stopped)
                ViewModel.PlayPause = "play";
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

            _canProgress = true;
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

            var slider = (Slider)sender;

            TimeSpan newTimeSong = TimeSpan.FromSeconds(slider.Value);
            string minutes = Math.Floor(newTimeSong.TotalMinutes).ToString("00");
            string seconds = newTimeSong.Seconds.ToString("00");

            ViewModel.ProgressLengthSong = $"{minutes}:{seconds}";
        }

        /// <summary>
        /// Event for when drag started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progresSlider_DragStarted(object sender, EventArgs e)
        {
            _canProgress = false;
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