using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;

namespace Dependencies.Exchange.Graph
{
    public class GraphImportAssembly : IImportAssembly
    {
        public Task<(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)> ImportAsync()
        {
            throw new NotImplementedException();
        }

    }
}
