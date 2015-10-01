using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace App1.Services
{
    public class PortabilityFactory : IPortabilityFactory
    {
        public ObservableCollection<T> GetIncrementalCollection<T>(int take, Func<int, Task<List<T>>> loadMoreItems, Action onBatchStart, Action<List<T>> onBatchComplete)
        {
            return new IncrementalLoadingCollection<T>(take, loadMoreItems, onBatchStart, onBatchComplete);
        }
    }
}