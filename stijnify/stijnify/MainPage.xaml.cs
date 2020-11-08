
using Plugin.FilePicker;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using stijnify.Interfaces;
using stijnify.Model;
using stijnify.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace stijnify
{
    public partial class MainPage : TabbedPage
    {
        public QueueInfo _Queue;

        public MainPage()
        {
            InitializeComponent();
            _Queue = new QueueInfo();
        }
    }
}
