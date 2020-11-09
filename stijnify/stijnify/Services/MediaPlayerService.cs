using MediaManager;
using MediaManager.Playback;
using ReactiveUI;
using stijnify.Interfaces;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private void MediaPlayer_MediaItemFinished(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            //Check if MediaPlayer has next item
            if (!HasNext())
            {
                Stop();
                return;
            }

            var queue = ((MainPage)Application.Current.MainPage)._Queue;
            queue._SelectedSong++;
            var song = queue._StandardQueue[queue._SelectedSong];

            Play(queue._StandardQueue[queue._SelectedSong].Path);
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
            var queue = mainPage._Queue;

            queue._StandardQueue = standardQueue;

            queue._SelectedSong = standardQueue.IndexOf(song);

            await Constants.MediaPlayer.Play(song.Path);
        }

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

        public bool HasNext()
        {
            var queue = ((MainPage)Application.Current.MainPage)._Queue;

            return queue._StandardQueue.Count > queue._SelectedSong + 1;
        }

        public async void ChangePosition(TimeSpan position)
        {
            await Constants.MediaPlayer.SeekTo(position);
        }
    }
}
