using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;

namespace Dependencies.Exchange.Graph
{
    public class GraphExportAssembly : IExportAssembly
    {
        public async Task ExportAsync(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)
        {
            var service = new AssemblyGraphService();

            await service.AddAsync(assembly, dependencies.Where(x => x.IsLocal).ToList());
        }
    }
}
