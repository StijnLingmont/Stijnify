using MediaManager;
using MediaManager.Playback;
using stijnify.Interfaces;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Services
{
    class MediaPlayerService : IMediaPlayer
    {
        public MediaPlayerService()
        {

        }

        public static async void SelectSong(string path)
        {
            await Constants.MediaPlayer.Play(path);
        }

        public async void Play(string path)
        {
            await Constants.MediaPlayer.Play(path);
        }

        public void Resume()
        {

        }

        public void Pause()
        {

        }

        public async void PlayPauseToggle()
        {
            await Constants.MediaPlayer.PlayPause();
        }
    }
}
