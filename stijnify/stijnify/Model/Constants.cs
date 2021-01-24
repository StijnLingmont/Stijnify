using MediaManager;
using System;
using System.Collections.Generic;
using System.Text;

namespace stijnify.Model
{
    public class Constants
    {
        //The mediaplayer
        public static IMediaManager MediaPlayer = CrossMediaManager.Current;
    }
}
