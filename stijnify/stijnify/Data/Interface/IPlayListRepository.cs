using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace stijnify.Data.Interface
{
    public interface IPlayListRepository
    {
        List<PlayListModel> GetPlayLists();
        void AddPlayList(PlayListModel playlist);
    }
}
