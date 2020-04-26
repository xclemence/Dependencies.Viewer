using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dependencies.Exchange.Base.Models;

namespace Dependencies.Exchange.Base
{
    public interface IImportAssembly
    {
        Task<(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)> ImportAsync();
    }
}
