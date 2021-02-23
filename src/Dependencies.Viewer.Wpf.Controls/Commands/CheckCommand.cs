using System.Linq;
using System.Threading.Tasks;
using Dependencies.Check;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.Services;
using Dependencies.Viewer.Wpf.Controls.Views.Check;
using MaterialDesignThemes.Wpf;

namespace Dependencies.Viewer.Wpf.Controls.Commands
{
    public class CheckCommand
    {
        private readonly MainBusyService busyService;

        public CheckCommand(MainBusyService busyService)
        {
            this.busyService = busyService;
        }

        public async Task CircularDependenciesCheck(AssemblyModel assembly)
        {
            await busyService.RunActionAsync(async () =>
            {
                var assemblies = assembly.ReferenceProvider.Select(x => x.Value.LoadedAssembly)
                                                           .Distinct()
                                                           .Select(x => x.ToCheckModel())
                                                           .ToDictionary(x => x.Name);

                var service = new CircularReferenceCheck();

                var result = service.Analyse(assembly.Name, assemblies).ToList();

                var view = new CircularDependenciesView
                {
                    DataContext = result
                };

                await DialogHost.Show(view).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
