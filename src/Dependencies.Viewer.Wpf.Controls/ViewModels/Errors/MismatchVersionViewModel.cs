using System.Collections.Generic;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.Services;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Errors
{
    public class MismatchVersionViewModel : ErrorListViewModel
    {
        public MismatchVersionViewModel(MainViewIdentifier mainViewIdentifier) : base(mainViewIdentifier)
        {
        }

        public override string Title => "Mismatch Version";

        protected override IEnumerable<ReferenceModel> GetResults(AssemblyModel? assembly)
        {
            if (assembly is null)
                return Enumerable.Empty<ReferenceModel>();

            return assembly.ReferenceProvider.Values.Where(x => x.AssemblyVersion != x.LoadedAssembly.Version)
                                                     .OrderBy(x => x.AssemblyFullName);
        }
    }
}