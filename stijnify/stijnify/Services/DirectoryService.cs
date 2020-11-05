using Plugin.Permissions.Abstractions;
using stijnify.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace stijnify.Services
{
    public class DirectoryService
    {
        public static async Task<string> GetRootDirectory()
        {
            //bool permissionStatus = await PermissionService.GrantReadPermission();

            //if (permissionStatus == false)
            //    Thread.CurrentThread.Abort();

            return DependencyService.Get<IExternalStorage>().GetExternalStorage();
        }
    }
}
