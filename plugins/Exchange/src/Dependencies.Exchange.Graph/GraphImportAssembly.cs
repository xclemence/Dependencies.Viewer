using System.Collections.Generic;
using System.Threading.Tasks;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;
using Dependencies.Exchange.Graph.ViewModels;
using Dependencies.Exchange.Graph.Views;

namespace Dependencies.Exchange.Graph
{
    public class GraphImportAssembly : IImportAssembly
    {
        public Task<(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)> ImportAsync()
        {
            var dataContext = new OpenAssemblyWindowModel();

            var window = new OpenAssemblyWindow { DataContext = dataContext };

            var result = window.ShowDialog();

            if (!(result ?? false))
                return default;

            return Task.FromResult(dataContext.AssemblyLoaded);
        }
    }
}
