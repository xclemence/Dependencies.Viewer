using System.Collections.Generic;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Errors
{
    public class MismatchVersionViewModel : ErrorListViewModel
    {
        public override string Title => "Mismatch Version";

        protected override IEnumerable<ReferenceModel> GetResults(AssemblyModel assembly)
        {
            return assembly?.ReferenceProvider.Values.Where(x => x.AssemblyVersion != x.LoadedAssembly.Version)
                                                      .OrderBy(x => x.AssemblyFullName);
        }
    }
}