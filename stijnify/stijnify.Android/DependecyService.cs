using System;
using System.Collections.Generic;
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
        public string GetExternalStorage()
        {
            try
            {
                string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string context = (string)Android.OS.Environment.ExternalStorageDirectory;

                return context;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}