using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Dependencies.Exchange.Base.Models;
using Dependencies.Exchange.Graph.Fwk;

namespace Dependencies.Exchange.Graph.ViewModels
{
    public class OpenAssemblyWindowModel : ObservableObject
    {
        private string searchText;
        private IList<string> availableAssemblies;
        private bool isBusy;
        private string selectedAssembly;

        public OpenAssemblyWindowModel()
        {
            SearchCommand = new Command(async () => await SearchAsync(), () => !string.IsNullOrEmpty(SearchText));
            CancelCommand = new Command<Window>(x => x.Close());
            LoadCommand = new Command<Window>(async (x) => await LoadAsync(x), x => !string.IsNullOrEmpty(SelectedAssembly));
        }

        public ICommand SearchCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand CancelCommand { get; }

        public bool IsBusy
        {
            get => isBusy;
            set => Set(ref isBusy, value);
        }

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

        public (AssemblyExchange assembly, IList<AssemblyExchange> dependencies) AssemblyLoaded { get; private set; }

        private async  Task SearchAsync()
        {
            try
            {
                IsBusy = true;
                var services = new AssemblyGraphService();
                AvailableAssemblies = await services.SearchAsync(SearchText);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadAsync(Window mainWindow)
        {
            try
            {
                IsBusy = true;
                var service = new AssemblyGraphService();
                AssemblyLoaded = await service.GetAsync(SelectedAssembly);
            }
            finally
            {
                IsBusy = false;
            }

            mainWindow.DialogResult = true;
            mainWindow.Close();
        }
    }
}
