using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using App1.Model;
using App1.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;

namespace App1.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region # prop

        public RelayCommand PageLoaded { get; set; }

        protected IPortabilityFactory factory
        {
            get
            {
                return SimpleIoc.Default.GetInstance<IPortabilityFactory>();
            }
        }

        private ObservableCollection<PostsEntity> _dataListLoader;
        public ObservableCollection<PostsEntity> DataListLoader
        {
            get { return _dataListLoader; }
            set
            {
                Set(() => DataListLoader, ref _dataListLoader, value);
            }
        }

        #endregion
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            WireCommands();
        }

        private async void WireCommands()
        {
            PageLoaded = new RelayCommand(async () =>
            {
                LoadEverything(false);
            });
        }


        private async Task LoadEverything(bool isRefresh)
        {

                //App.LoadedGalleryImgs =
            DataListLoader = factory.GetIncrementalCollection<PostsEntity>(1,
                GetMainPageFeedLoader, //<- Method Querying
                () =>
                {
                    //<- Start Query
                }, 
                (data) =>
                {
                    // <- End Query
                }); 
        }


        public async Task<List<PostsEntity>> GetMainPageFeedLoader(int pageNumber)
        {
            //Make a call to get more data
            //var rootElement = await ApiRoot.GetDataPage(pageNumber);
            //return rootElement.Select(data => new PostsEntityViewModel(data)).ToList();

            var Element = new List<PostsEntity>();

            for (int i = 0; i < 10; i++)
            {
                Element.Add( new PostsEntity()
                {
                    ImageUrl = "Image Url",
                    Index = pageNumber,
                    Text = string.Format("Page index {0}, forechIndex {1}", pageNumber, i)
                });
            }

            return Element;
        }

    }
}