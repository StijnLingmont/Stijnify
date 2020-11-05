using ReactiveUI;
using stijnify.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace stijnify.ViewModels
{
    class HomePageModel : ReactiveObject, INotifyPropertyChanged
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

        public HomePageModel()
        {
            SongList = new ObservableCollection<SongInfoModel>();
        }
    }
}
