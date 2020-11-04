using stijnify.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        #endregion

        public HomePage()
        {
            InitializeComponent();

            _RootFolder = DependencyService.Get<IExternalStorage>().GetExternalStorage();

            GetAllFiles();
        }

        /// <summary>
        /// Get All Files from a Static Folder from my own phone.
        /// </summary>
        void GetAllFiles()
        {
            try
            {
                //TODO: Instead of adding here use binding
                ObservableCollection<string> allSongs = new ObservableCollection<string>();

                //TODO: Get list of all folders
                //TODO: Get for every folder in the list all the files
                string songsReadPath = _RootFolder + "/Music/Serie themes/";

                var allFiles = Directory.GetFiles(songsReadPath);

                //Go trough all files
                foreach (var file in allFiles)
                {
                    //Check if file is an supported file
                    if (Path.GetExtension(file) == ".mp3")
                    {
                        allSongs.Add(file);
                    }
                }

                songList.ItemsSource = allSongs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            };
        }
    }
}