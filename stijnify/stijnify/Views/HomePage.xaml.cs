using Xamarin.Essentials;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using stijnify.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using stijnify.Services;
using stijnify.ViewModels;
using stijnify.Model;

namespace stijnify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        #region Initialise Variables

        /// <summary>
        /// Root Folder of the device
        /// </summary>
        string _RootFolder;

        HomePageModel ViewModel;

        #endregion

        public HomePage()
        {
            InitializeComponent();

            _RootFolder = DirectoryService.GetRootDirectory().Result;

            BindingContext = ViewModel = new HomePageModel();

            GetAllFiles();
        }

        /// <summary>
        /// Get All Files from the folders chosen in the Settings
        /// </summary>
        void GetAllFiles()
        {
            try
            {
                ObservableCollection<SongInfoModel> allSongs = new ObservableCollection<SongInfoModel>();
                ObservableCollection<string> folderList;

                //Get all selected folders
                var folders = Preferences.Get("folders", null);

                if (folders == null)
                    return;

                folderList = new ObservableCollection<string>(folders.Split(','));

                //Go trough every folder and get all files
                foreach(string folder in folderList)
                {
                    //Retrieve all files with the Extension .mp3
                    var allFiles = Directory.GetFiles(folder).Where(file => Path.GetExtension(file) == ".mp3");

                    //Add every item in the list of songs
                    foreach (string file in allFiles)
                        allSongs.Add(new SongInfoModel(Path.GetFileNameWithoutExtension(file), file));
                }

                ViewModel.SongList = allSongs;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };
        }

        /// <summary>
        /// Event for refresh song list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void songListView_Refreshing(object sender, EventArgs e)
        {
            var listView = (ListView)sender;

            GetAllFiles();

            listView.EndRefresh();
        }
    }
}