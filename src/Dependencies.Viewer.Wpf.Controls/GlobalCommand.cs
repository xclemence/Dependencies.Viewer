using System;
using System.Threading.Tasks;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls
{
    public static class GlobalCommand
    {
        internal static Action<AssemblyModel>? OpenAssemblyAction { get; set; }
        internal static Func<AssemblyModel, ReferenceModel, Task>? ViewParentReference { get; set; }

        public static void OpenAssembly(AssemblyModel assembly) => OpenAssemblyAction?.Invoke(assembly);

        public static async Task ViewParentReferenceAsync(AssemblyModel? baseAssembly, ReferenceModel reference)
        {
            if (baseAssembly is null || ViewParentReference is null)
                return;

            await ViewParentReference.Invoke(baseAssembly, reference).ConfigureAwait(false);
        }
    }
}
