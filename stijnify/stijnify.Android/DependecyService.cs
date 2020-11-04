using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using stijnify.Droid;
using stijnify.Interfaces;
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
                var test = System.IO.Directory.GetDirectories("/storage/emulated/0");


                return context;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    }
}