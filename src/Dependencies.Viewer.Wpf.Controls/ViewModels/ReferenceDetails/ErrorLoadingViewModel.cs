using System.Collections.Generic;
using System.Linq;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.ReferenceDetails
{
    public class ErrorLoadingViewModel : ReferenceListViewModel
    {
        public override string Title => "Error Loading";

        protected override IEnumerable<ReferenceModel> GetResults(AssemblyInformation information)
        {
            return information?.GetAllLinks()
                               .Distinct()
                               .Where(x => !x.Assembly.IsResolved)
                               .Select(x => new ReferenceModel(x))
                               .OrderBy(x => x.Link.Assembly.Name);
        }
    }
}
