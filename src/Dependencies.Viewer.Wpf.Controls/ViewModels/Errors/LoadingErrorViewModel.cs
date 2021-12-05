using System.Collections.Generic;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Models;
using Dependencies.Viewer.Wpf.Controls.Services;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Errors
{
    public class LoadingErrorViewModel : ErrorListViewModel
    {
        public LoadingErrorViewModel(MainViewIdentifier mainViewIdentifier) : base(mainViewIdentifier)
        {
        }

        public override string Title => "Loading Errors";

        protected override IEnumerable<ReferenceModel> GetResults(AssemblyModel? assembly) =>
            assembly?.ReferenceProvider.Values.Where(x => !x.LoadedAssembly.IsResolved).OrderBy(x => x.AssemblyFullName) ?? Enumerable.Empty<ReferenceModel>();
    }
}
