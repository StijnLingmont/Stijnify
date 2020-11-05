using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace stijnify.Services
{
    public class PermissionService
    {
        public static async Task<bool> GrantReadPermission()
        {
			try
			{
				var status = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
				if (status != PermissionStatus.Granted)
				{
					if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
					{
                        Console.WriteLine("Already declined");
					}

					status = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
				}

				if (status == PermissionStatus.Granted)
					return true;

				return false;
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex);
				return false;
				//Something went wrong
			}
		}
    }
}
