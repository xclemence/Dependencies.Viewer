using System;
using System.Threading.Tasks;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.ViewModels;
using MaterialDesignThemes.Wpf;

namespace Dependencies.Viewer.Wpf.Controls
{
    public static class GlobalCommand
    {
        internal static Action<AssemblyModel> OpenAssemblyAction { get; set; }

        public static void OpenAssembly(AssemblyModel assembly) => OpenAssemblyAction?.Invoke(assembly);

        public static async Task ViewParentReferenceAsync(AssemblyModel baseAssembly, ReferenceModel reference)
        {
            var vm = new AssemblyParentsViewModel(reference.LoadedAssembly, baseAssembly);

            _ = await DialogHost.Show(vm).ConfigureAwait(false);
        }
    }
}
