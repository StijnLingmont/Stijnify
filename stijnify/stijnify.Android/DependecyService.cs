using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using stijnify.Droid;
using stijnify.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(DependecyService))]
namespace stijnify.Droid
{
    class DependecyService : IExternalStorage
    {
        /// <summary>
        /// Get base paths.
        /// </summary>
        /// <returns>List of base paths, phone storage and sd card storage paths.</returns>
        public List<string> GetBasePaths()
        {
            // Base paths
            var basePaths = new List<string>();

            try
            {
                // Add internal phone storage
                basePaths.Add((string)Android.OS.Environment.ExternalStorageDirectory);

                // Storage manager
                var storageManager = (Android.OS.Storage.StorageManager)Android.App.Application.Context.GetSystemService(Context.StorageService);

                // All storage volumes
                var storageVolumes = (Java.Lang.Object[])storageManager.Class.GetDeclaredMethod("getVolumeList").Invoke(storageManager);

                // Add all sd cards
                foreach (var storage in storageVolumes)
                {
                    var info = (Java.IO.File)storage.Class.GetDeclaredMethod("getPathFile").Invoke(storage);

                    if ((bool)storage.Class.GetDeclaredMethod("isEmulated").Invoke(storage) == false && info.TotalSpace > 0)
                        basePaths.Add(info.Path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetBasePaths Got exception while trying to get base paths! Current list: {basePaths}. {ex}");
            }

            // Return base paths
            return basePaths;
        }
    }
}