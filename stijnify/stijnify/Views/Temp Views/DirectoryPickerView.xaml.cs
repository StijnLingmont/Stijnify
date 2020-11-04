using stijnify.Interfaces;
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

namespace stijnify.Views.Temp_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DirectoryPickerView : ContentPage
    {
        #region Init Variables

        /// <summary>
        /// The Folder currently selected
        /// </summary>
        private string _SelectedFolder;

        #endregion

        public DirectoryPickerView()
        {
            InitializeComponent();

            //Set root folder as selected folder
            var _SelectedFolder = DependencyService.Get<IExternalStorage>().GetExternalStorage();

            GetFolderStructure();
        }

        /// <summary>
        /// Get all folders in the selected Folder
        /// </summary>
        void GetFolderStructure()
        {
            try
            {
                ObservableCollection<string> allDirectories = new ObservableCollection<string>();

                var rootFolders = Directory.GetDirectories(_SelectedFolder);

                foreach (string folder in rootFolders)
                {
                    allDirectories.Add(folder);
                }

                directoriesList.ItemsSource = allDirectories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };
        }

        /// <summary>
        /// Event to open folder when folder has been selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void directoriesList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string folderSelected = (string)e.Item;

            _SelectedFolder = folderSelected;

            GetFolderStructure();
        }

        /// <summary>
        /// Event for canceling the Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelDialog(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        /// <summary>
        /// Select specefic directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDirectory(object sender, EventArgs e)
        {
            bool hasAlreadyFolders = Preferences.ContainsKey("folders");
            List<string> folders = new List<string>();

            if(hasAlreadyFolders)
            {
                //Retrieve data and store it in the list
                var foldersString = Preferences.Get("folders", null);
                folders = foldersString.Split(',').ToList();
            }

            //Go trough the full list of folders and check if the folder chosen already is added
            foreach(string excistingFolder in folders)
            {
                if (_SelectedFolder == excistingFolder)
                {
                    DisplayAlert("Folder already added", "The folder you tried to add already excists! Please choose another folder.", "OK");
                    return;
                }

            }

            //Add folder to list
            folders.Add(_SelectedFolder);

            //Set list in Prefferences
            Preferences.Set("folders", string.Join(",", folders));
            
            Navigation.PopModalAsync();
        }
    }
}