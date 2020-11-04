
using Plugin.FilePicker;
using stijnify.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace stijnify
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

            //TODO: REMOVE THIS! THIS IS TEMP FOR TESTING
            Preferences.Clear();
        }
    }
}
