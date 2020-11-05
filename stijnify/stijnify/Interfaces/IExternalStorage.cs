using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace stijnify.Interfaces
{
    public interface IExternalStorage
    {
        /// <summary>
        /// Get the path of the External Storage
        /// </summary>
        /// <returns>Path of External Storage</returns>
        string GetExternalStorage();
    }
}
