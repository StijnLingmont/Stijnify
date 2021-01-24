using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using stijnify.Services;
using stijnify.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stijnify
{
    public partial class App : Application
    {
        public App()
        {
            Container.Build();

            InitializeComponent();

            PermissionCheck();
        }

        private async void PermissionCheck()
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
            if (permissionStatus != PermissionStatus.Granted)
                MainPage = new AskPermission();
            else
                MainPage = new MainPage();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            base.OnSleep();
        }

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}
