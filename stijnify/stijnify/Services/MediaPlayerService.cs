using MediaManager;
using MediaManager.Playback;
using ReactiveUI;
using stijnify.Interfaces;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Xamarin.Forms;

namespace stijnify.Services
{
    public class MediaPlayerService : IMediaPlayerService
    {
        public MediaPlayerService()
        {
            Constants.MediaPlayer.MediaItemFinished += MediaPlayer_MediaItemFinished;
        }

        #region MediaPlayer Events

        /// <summary>
        /// Event for when song is Finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaPlayer_MediaItemFinished(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            Next();
        }

        #endregion

        /// <summary>
        /// Force Select a song
        /// </summary>
        /// <param name="song"></param>
        /// <param name="standardQueue"></param>
        public static async void SelectSong(SongInfoModel song, List<SongInfoModel> standardQueue)
        {
            //Get the queue
            MainPage mainPage = (MainPage)App.Current.MainPage;
            var queue = mainPage.QueueService;

            queue.ForcePlayItem(song, standardQueue);

            await Constants.MediaPlayer.Play(song.Path);
        }

        #region Basic song actions

        public async void Play(string path)
        {
            await Constants.MediaPlayer.Play(path);
        }

        public async void PlayPauseToggle()
        {
            await Constants.MediaPlayer.PlayPause();
        }

        public async void Stop()
        {
            await Constants.MediaPlayer.Stop();
        }

        public async void ChangePosition(TimeSpan position)
        {
            await Constants.MediaPlayer.SeekTo(position);
        }

        #endregion

        #region Next/Previous

        public void Next()
        {
            var queueService = ((MainPage)Application.Current.MainPage).QueueService;

            var nextSong = queueService.NextQueueItem();

            if (nextSong == null)
                Constants.MediaPlayer.Stop();
            else
                Play(nextSong.Path);
        }

        public void Previous()
        {
            var queueService = ((MainPage)Application.Current.MainPage).QueueService;

            var prevSong = queueService.PreviousQueueItem();

            if (prevSong == null)
                Constants.MediaPlayer.Stop();
            else
                Play(prevSong.Path);
        }

        #endregion
    }
}
