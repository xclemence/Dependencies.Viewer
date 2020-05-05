using System;
using System.Threading.Tasks;

namespace Dependencies.Exchange.Base
{
    public interface IExchangeViewModel<T>
    {
        bool CanLoad { get; }

        Func<Func<Task>, Task> RunAsync { get; set; }

        Task<T> LoadAsync();
    }

}
