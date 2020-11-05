using stijnify.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stijnify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AskPermission : ContentPage
    {
        public AskPermission()
        {
            InitializeComponent();
        }

        private async void AskPermission_Clicked(object sender, EventArgs e)
        {
            var status = await PermissionService.GrantReadPermission();

            // Throw Exception When used denied the permission
            if(!status)
                throw new Exception();

            Application.Current.MainPage = new MainPage();
        }
    }
}