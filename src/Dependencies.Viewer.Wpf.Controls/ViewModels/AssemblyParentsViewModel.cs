using System.Collections.Generic;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AssemblyParentsViewModel
    {

        public AssemblyParentsViewModel(AssemblyModel assembly, AssemblyModel rootAssembly)
        {
            Assembly = assembly;
            RootAssembly = rootAssembly;

            Paths = Assembly.ParentLinkNames.Select(x => new AssemblyRevertLinkItem(AssemblyProvider(x), AssemblyProvider, true)).ToList();
        }

        public AssemblyModel Assembly { get; set; }
        public AssemblyModel RootAssembly { get; set; }

        public string AssemblyName => Assembly.FullName;

        public IList<AssemblyRevertLinkItem> Paths { get; set; }

        private AssemblyModel AssemblyProvider(string assemblyName)
        {
            if (RootAssembly.FullName == assemblyName)
                return RootAssembly;

            return RootAssembly.ReferenceProvider[assemblyName].LoadedAssembly;
        }

    }
}
