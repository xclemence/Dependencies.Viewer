using System.Threading.Tasks;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.Services;
using Dependencies.Viewer.Wpf.Controls.ViewModels;
using MaterialDesignThemes.Wpf;

namespace Dependencies.Viewer.Wpf.Controls.Commands;

public class OpenCommand
{
    private readonly MainBusyService busyService;
    private readonly AnalyserViewModel analyserViewModel;

    public OpenCommand(MainBusyService busyService, AnalyserViewModel analyserViewModel)
    {
        this.busyService = busyService;
        this.analyserViewModel = analyserViewModel;
    }

    public async Task ViewParentReferenceAsync(AssemblyModel? baseAssembly, ReferenceModel reference)
    {
        if (baseAssembly is null)
            return;

        await busyService.RunActionAsync(async () =>
        {
            var vm = new AssemblyParentsViewModel(reference.LoadedAssembly, baseAssembly);

            _ = await DialogHost.Show(vm).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    public Task OpenSubAssembly(AssemblyModel assembly) =>
        Task.Run(() => busyService.RunAction(() => analyserViewModel.AddAssemblyResult(assembly.IsolatedShadowClone())));

}
