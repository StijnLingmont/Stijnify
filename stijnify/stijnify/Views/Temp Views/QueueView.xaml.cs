using Autofac;
using stijnify.Interfaces;
using stijnify.Model;
using stijnify.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stijnify.Views.Temp_Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QueueView : ContentPage
    {
        IMediaPlayerService _mediaPlayerService;
        QueueService _queueService;
        QueueModel ViewModel;

        public QueueView()
        {
            InitializeComponent();

            _mediaPlayerService = Container.ContainerInstance.Resolve<IMediaPlayerService>();
            _queueService = ((MainPage)Application.Current.MainPage).QueueService;

            BindingContext = ViewModel = new QueueModel();

            InitQueue();
        }

        private void InitQueue()
        {
            ViewModel.CustomQueue = new ObservableCollection<SongInfoModel>(_queueService._queue._CustomQueue);
        }

        private void QueueItemRemove_Clicked(object sender, EventArgs e)
        {
            SongInfoModel songInfo = (SongInfoModel)((ImageButton)sender).CommandParameter;
            var songIndex = _queueService._queue._CustomQueue.IndexOf(songInfo);

            _queueService.RemoveCustomQueueItem(songInfo);
            InitQueue();
            
            if(songIndex == _queueService._queue._CustomSelectedSong)
            {
                var newSong = _queueService.GetQueueItem();
                _mediaPlayerService.Play(newSong.Path);
            }
        }
    }
}