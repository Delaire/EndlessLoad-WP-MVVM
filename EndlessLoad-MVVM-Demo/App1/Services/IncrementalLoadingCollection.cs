using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace App1.Services
{
    public class IncrementalLoadingCollection<T>
        : ObservableCollection<T>, ISupportIncrementalLoading
    {
        private Func<int, Task<List<T>>> _loadMoreItems = null;
        private Action<List<T>> _onBatchComplete = null;
        private Action _onBatchStart = null;

        /// <summary>
        /// Max times allowed to query
        /// </summary>
        private int _maxItems = 10;

        /// <summary>
        /// Page Index to Query start at 1
        /// </summary>
        private int pageIndex = 1;

        private int PageIndex
        {
            get
            {
                return pageIndex;
            }
            set
            {
                pageIndex = value;
            }
        }

        /// <summary>
        /// The max number of items to get per batch
        /// </summary>
        private int Take { get; set; }

        /// <summary>
        /// The number of items in the last batch retrieved
        /// </summary>
        private int VirtualCount { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="take">How many items to take per batch</param>
        /// <param name="loadMoreItems">The load more items function</param>
        public IncrementalLoadingCollection(int take, Func<int, Task<List<T>>> loadMoreItems, Action onBatchStart, Action<List<T>> onBatchComplete)
        {
            Take = take;
            _loadMoreItems = loadMoreItems;
            _onBatchStart = onBatchStart;
            _onBatchComplete = onBatchComplete;
            VirtualCount = take;
            this.hasMoreItems = true;
        }

        /// <summary>
        /// Returns whether there are more items (if the current batch size is equal to the amount retrieved then YES)
        /// </summary>
        //public bool HasMoreItems
        //{
        //    get { return this.VirtualCount >= Take; }
        //}
        private bool hasMoreItems;
        public bool HasMoreItems
        {
            get { return hasMoreItems && this.PageIndex < _maxItems; }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            CoreDispatcher dispatcher = Window.Current.Dispatcher;
            //  CoreDispatcher dispatcher = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher; ;
            _onBatchStart(); // This is the UI thread

            return Task.Run<LoadMoreItemsResult>(
                 async () =>
                 {
                     var result = await _loadMoreItems(PageIndex);

                     if (result == null || result.Count() == 0)
                     {
                         hasMoreItems = false;
                     }
                     else
                     {
                         this.VirtualCount = result.Count;
                         PageIndex += Take;

                         await dispatcher.RunAsync(
                             CoreDispatcherPriority.High,
                             () =>
                             {
                                 foreach (T item in result) this.Add(item);
                                 _onBatchComplete(result); // This is the UI thread
                             });
                     }
                     return new LoadMoreItemsResult() { Count = (uint)result.Count };

                 }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }
}
