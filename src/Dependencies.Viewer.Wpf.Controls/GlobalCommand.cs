using System;
using System.Linq;
using System.Threading.Tasks;
using Dependencies.Viewer.Wpf.Controls.Extensions;
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
            var paths = reference.GetAssemblyParentPath(baseAssembly).ToList();
            var vm = new AssemblyParentsViewModel { BaseAssembly = reference.AssemblyFullName, Paths = paths };

            _ = await DialogHost.Show(vm).ConfigureAwait(false);
        }
    }
}
