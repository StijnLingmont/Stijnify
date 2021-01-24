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
        #region Init variables
        
        /// <summary>
        /// All actions for the mediaplayer
        /// </summary>
        IMediaPlayerService _mediaPlayerService;

        /// <summary>
        /// All actions for the queues
        /// </summary>
        QueueService _queueService;

        /// <summary>
        /// The viewmodel of this contentpage
        /// </summary>
        QueueModel ViewModel;

        #endregion

        #region Init

        public QueueView()
        {
            InitializeComponent();

            _mediaPlayerService = Container.ContainerInstance.Resolve<IMediaPlayerService>();
            _queueService = ((MainPage)Application.Current.MainPage).QueueService;

            BindingContext = ViewModel = new QueueModel();

            InitQueue();
        }

        /// <summary>
        /// Init the queue
        /// </summary>
        private void InitQueue()
        {
            ViewModel.CustomQueue = new ObservableCollection<SongInfoModel>(_queueService._queue._CustomQueue);
        }

        #endregion

        /// <summary>
        /// Remove an item from the queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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