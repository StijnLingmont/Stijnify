using stijnify.Interfaces;
using stijnify.Views.Temp_Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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

        /// <summary>
        /// Event for folder options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FolderDelete_Clicked(object sender, EventArgs e)
        {
            string selectedItem = ((ImageButton)sender).CommandParameter.ToString();
            var response = await DisplayAlert("Deleting folder", "Are you sure that you want to delete this folder?", "Yes", "No");

            //Check if user want to proceed
            if (response)
            {
                //Delete the folder
                List<string> folders = new List<string>();

                //Retrieve the full string with data
                var foldersString = Preferences.Get("folders", null);

                //Check if list is found
                if(foldersString == null)
                    return;

                //Convert the string in a list
                folders = foldersString.Split(',').ToList();

                //Find specefic folder
                bool folderExcists = folders.Any(folder => folder == selectedItem);

                if (folderExcists)
                    folders.Remove(selectedItem);

                //Check if there is still an item left
                if (folders.Count > 0)
                    Preferences.Set("folders", string.Join(",", folders));
                else
                    Preferences.Remove("folders");

                addedFolders.ItemsSource = folders;
            }
        }
    }
}