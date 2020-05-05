using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using Dependencies.Exchange.Base.Models;

namespace Dependencies.Exchange.Base
{
    public interface IExportAssembly : IExchangeSevice
    {
        Task ExportAsync(AssemblyExchange assembly, IList<AssemblyExchange> dependencies, Func<UserControl, bool> viewCaller);
    }
}
