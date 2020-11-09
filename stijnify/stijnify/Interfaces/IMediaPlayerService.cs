using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Interfaces
{
    interface IMediaPlayerService
    {
        /// <summary>
        /// Play song
        /// </summary>
        /// <param name="path"></param>
        void Play(string path);

        /// <summary>
        /// Stop the mediaplayer
        /// </summary>
        void Stop();

        /// <summary>
        /// Toggle for the play and pause
        /// </summary>
        void PlayPauseToggle();

        /// <summary>
        /// Go to next song
        /// </summary>
        void Next();

        /// <summary>
        /// Go to previous song
        /// </summary>
        void Previous();

        /// <summary>
        /// Change position of song
        /// </summary>
        /// <param name="position"></param>
        void ChangePosition(TimeSpan position);
    }
}
