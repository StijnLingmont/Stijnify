using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Interfaces
{
    interface IMediaPlayer
    {
        /// <summary>
        /// Play song
        /// </summary>
        /// <param name="path"></param>
        void Play(string path);

        /// <summary>
        /// Toggle for the play and pause
        /// </summary>
        void PlayPauseToggle();
    }
}
