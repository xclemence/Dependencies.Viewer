using System.Threading.Tasks;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.Services;
using MaterialDesignThemes.Wpf;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Errors
{
    public abstract class ErrorListViewModel : ResultListViewModel<ReferenceModel>
    {
        private readonly MainViewIdentifier mainViewIdentifier;

        public ErrorListViewModel(MainViewIdentifier mainViewIdentifier)
        {
            this.mainViewIdentifier = mainViewIdentifier;
        }

        protected override async Task OnOpenResultAsync(ReferenceModel item)
        {
            if (Assembly is null)
                return;

            var vm = new AssemblyParentsViewModel(item.LoadedAssembly, Assembly);

            _ = await DialogHost.Show(vm, mainViewIdentifier.Id).ConfigureAwait(false);
        }
    }
}