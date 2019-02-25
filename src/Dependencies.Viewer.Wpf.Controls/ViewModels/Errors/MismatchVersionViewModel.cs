using System.Collections.Generic;
using System.Linq;
using Dependencies.Analyser.Base.Extensions;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.Errors
{
    public class MismatchVersionViewModel : ErrorListViewModel
    {
        public override string Title => "Mismatch Version";

        protected override IEnumerable<ReferenceModel> GetResults(AssemblyInformation information)
        {
            return information?.GetAllLinks()
                               .Where(x => x.LinkVersion != x.Assembly.LoadedVersion)
                               .Distinct()
                               .Select(x => new ReferenceModel(x))
                               .OrderBy(x => x.ToString());
        }
    }
}