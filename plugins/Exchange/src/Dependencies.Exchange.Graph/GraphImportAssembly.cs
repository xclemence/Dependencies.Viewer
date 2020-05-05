using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;
using Dependencies.Exchange.Graph.Settings;
using Dependencies.Exchange.Graph.ViewModels;
using Dependencies.Exchange.Graph.Views;

namespace Dependencies.Exchange.Graph
{
    public class GraphImportAssembly : IImportAssembly
    {
        private readonly ISettingServices<GraphSettings> settings;

        public string Name => "Graph";
        public bool IsReady => true;

        public GraphImportAssembly(ISettingServices<GraphSettings> settings) => this.settings = settings;

        public async Task<AssemblyExchangeContent> ImportAsync(Func<UserControl, IExchangeViewModel<AssemblyExchangeContent>, Task<AssemblyExchangeContent>> showDialog)
        {
            var dataContext = new OpenAssemblyViewModel(settings);
            var window = new OpenAssemblyView();
            
            var result = await showDialog(window, dataContext);

            return result;
        }
    }
}
