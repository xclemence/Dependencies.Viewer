using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;
using Dependencies.Exchange.Graph.Fwk;

namespace Dependencies.Exchange.Graph.ViewModels
{
    public class OpenAssemblyViewModel : ObservableObject, IExchangeViewModel<AssemblyExchangeContent>
    {
        private string searchText;
        private IList<string> availableAssemblies;
        private string selectedAssembly;
        private readonly GraphSettings settings;

        public OpenAssemblyViewModel(GraphSettings settings)
        {
            this.settings = settings;

            SearchCommand = new Command(async () => await SearchAsync(), () => !string.IsNullOrEmpty(SearchText));
        }

        public ICommand SearchCommand { get; }

        public bool CanLoad => !string.IsNullOrEmpty(SelectedAssembly);

        public string SearchText
        {
            get => searchText;
            set => Set(ref searchText, value);
        }

        public string SelectedAssembly
        {
            get => selectedAssembly;
            set => Set(ref selectedAssembly, value);
        }

        public IList<string> AvailableAssemblies
        {
            get => availableAssemblies;
            set => Set(ref availableAssemblies, value);
        }

        public Func<Func<Task>, Task> RunAsync { get; set; }

        public string Title => "Open assembly from Graph Services";

        private async Task SearchAsync()
        {
            await RunAsync?.Invoke(async () =>
            {
              var services = new AssemblyGraphService(settings);
              AvailableAssemblies = await services.SearchAsync(SearchText);
            });
        }

        public async Task<AssemblyExchangeContent> LoadAsync()
        {
            var service = new AssemblyGraphService(settings);
            var (assembly, dependencies) = await service.GetAsync(SelectedAssembly);

            return new AssemblyExchangeContent { Assembly = assembly, Dependencies = dependencies };
        }
    }
}
