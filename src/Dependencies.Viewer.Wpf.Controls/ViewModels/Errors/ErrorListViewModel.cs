using System.Linq;
using System.Threading.Tasks;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;
using MaterialDesignThemes.Wpf;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Errors
{
    public abstract class ErrorListViewModel : ResultListViewModel<ReferenceModel>
    {
        protected override async Task OnOpenResultAsync(ReferenceModel item)
        {
            var paths = AssemblyInformation.GetAssemblyParentPath(item.Link).ToList();
            var vm = new AssemblyParentsViewModel { BaseAssembly = item.Link.Assembly.FullName, Paths = paths };

            var result = await DialogHost.Show(vm);
        }
    }
}