using System;
using System.Linq;
using System.Threading.Tasks;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.ViewModels;
using MaterialDesignThemes.Wpf;

namespace Dependencies.Viewer.Wpf.Controls
{
    public static class GlobalCommand
    {
        internal static Action<AssemblyInformation> OpenAssemblyAction { get; set; }

        public static void OpenAssembly(AssemblyInformation assembly)
        {
            OpenAssemblyAction?.Invoke(assembly);
        }

        public static async Task ViewParentReferenceAsync(AssemblyInformation baseAssembly, AssemblyLink searchLink)
        {
            var paths = baseAssembly.GetAssemblyParentPath(searchLink).ToList();
            var vm = new AssemblyParentsViewModel { BaseAssembly = searchLink.Assembly.FullName, Paths = paths };

            var result = await DialogHost.Show(vm);
        }
    }
}
