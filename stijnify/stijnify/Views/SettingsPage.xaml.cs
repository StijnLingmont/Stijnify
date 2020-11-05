using stijnify.Interfaces;
using stijnify.Views.Temp_Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stijnify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            SetFolderList();
        }

        /// <summary>
        /// Event to add Folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void AddFolder_Clicked(object sender, EventArgs e)
        {
            var DirectoryPickerView = new DirectoryPickerView();

            //Event for when DirectoryPicker is closed
            DirectoryPickerView.Disappearing += (sender2, e2) =>
            {
                SetFolderList();
            };

            await Navigation.PushModalAsync(DirectoryPickerView);
        }

        /// <summary>
        /// Sets the list of all the Filers in the ListView
        /// </summary>
        void SetFolderList()
        {
            if(Preferences.ContainsKey("folders"))
            {
                var folders = Preferences.Get("folders", null);

                ObservableCollection<string> folderList = new ObservableCollection<string>(folders.Split(','));

                addedFolders.ItemsSource = folderList;
            }

        }
    }
}