using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;
using Dependencies.Exchange.Graph.Extensions;
using Dependencies.Exchange.Graph.Settings;

namespace Dependencies.Exchange.Graph
{
    public class GraphExportAssembly : IExportAssembly
    {
        private readonly ISettingServices<GraphSettings> settings;

        public string Name => "Graph";
        public bool IsReady => settings.GetSettings().IsValide();

        public GraphExportAssembly(ISettingServices<GraphSettings> settings) => this.settings = settings;

        public async Task ExportAsync(AssemblyExchange assembly,
                                      IList<AssemblyExchange> dependencies,
                                      Func<UserControl, IExchangeViewModel<AssemblyExchangeContent>, Task<AssemblyExchangeContent>> _)
        {
            var service = new AssemblyGraphService(settings.GetSettings());

            await service.AddAsync(assembly, dependencies.ToList());
        }
    }
}
