using System;
using System.Threading.Tasks;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls
{
    public static class GlobalCommand
    {
        internal static Action<AssemblyModel> OpenAssemblyAction { get; set; }
        internal static Func<AssemblyModel, ReferenceModel, Task> ViewParentReference { get; set; }

        public static void OpenAssembly(AssemblyModel assembly) => OpenAssemblyAction?.Invoke(assembly);

        public static Task ViewParentReferenceAsync(AssemblyModel baseAssembly, ReferenceModel reference) => ViewParentReference.Invoke(baseAssembly, reference);
    }
}
