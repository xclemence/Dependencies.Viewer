using System.Collections.Generic;
using System.Threading.Tasks;
using Dependencies.Exchange.Base.Models;

namespace Dependencies.Exchange.Base
{
    public interface IExportAssembly
    {
        Task ExportAsync(AssemblyExchange assembly, IList<AssemblyExchange> dependencies);
    }
}
