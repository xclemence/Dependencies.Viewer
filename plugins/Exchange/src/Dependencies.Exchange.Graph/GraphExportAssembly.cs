using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;
using Dependencies.Exchange.Graph.Extensions;

namespace Dependencies.Exchange.Graph
{
    public class GraphExportAssembly : IExportAssembly
    {
        private readonly GraphSettings settings;

        public string Name => "Graph";
        public bool IsReady => settings.IsValide();

        public GraphExportAssembly(GraphSettings settings) => this.settings = settings;

        public async Task ExportAsync(AssemblyExchange assembly, IList<AssemblyExchange> dependencies, Func<UserControl, bool> dialogCaller)
        {
            var service = new AssemblyGraphService(settings);

            await service.AddAsync(assembly, dependencies.Where(x => x.IsLocal).ToList());
        }
    }
}
