using System.Threading.Tasks;
using Dependencies.Viewer.Wpf.Controls.Models;
using MaterialDesignThemes.Wpf;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Errors
{
    public abstract class ErrorListViewModel : ResultListViewModel<ReferenceModel>
    {
        protected override async Task OnOpenResultAsync(ReferenceModel item)
        {
            if (Assembly is null)
                return;

            var vm = new AssemblyParentsViewModel(item.LoadedAssembly, Assembly);

            _ = await DialogHost.Show(vm).ConfigureAwait(false);
        }
    }
}