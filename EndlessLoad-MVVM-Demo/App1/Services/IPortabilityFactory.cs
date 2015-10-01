using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IPortabilityFactory
    {
        ObservableCollection<T> GetIncrementalCollection<T>(int take, Func<int, Task<List<T>>> loadMoreItems, Action onBatchStart, Action<List<T>> onBatchComplete);
    }
}
