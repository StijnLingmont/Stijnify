using MediaManager;
using MediaManager.Playback;
using ReactiveUI;
using stijnify.Interfaces;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace stijnify.Services
{
    class MediaPlayerService : IMediaPlayer
    {
        public MediaPlayerService()
        {

        }

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

        public static async void ChangePosition(TimeSpan position)
        {
            await Constants.MediaPlayer.SeekTo(position);
        }
    }
}
