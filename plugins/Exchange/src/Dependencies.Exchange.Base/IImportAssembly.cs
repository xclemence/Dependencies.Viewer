using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using Dependencies.Exchange.Base.Models;

namespace Dependencies.Exchange.Base
{
    public interface IImportAssembly : IExchangeSevice
    {
        Task<AssemblyExchangeContent> ImportAsync(Func<UserControl, IExchangeViewModel<AssemblyExchangeContent>, Task<AssemblyExchangeContent>> showDialog);
    }
}
