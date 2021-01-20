using ReactiveUI;
using stijnify.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace stijnify.ViewModels
{
    class SongListModel : ReactiveObject
    {
        ObservableCollection<SongInfoModel> _songList;

        public ObservableCollection<SongInfoModel> SongList
        {
            get => _songList;
            set
            {
                _songList = value;
                OnPropertyChanged(nameof(SongList));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SongListModel()
        {
            SongList = new ObservableCollection<SongInfoModel>();
        }
    }
}
