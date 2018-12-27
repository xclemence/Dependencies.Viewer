using System.Collections.Generic;
using System.Linq;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.ReferenceDetails
{
    public class AllReferencesViewModel : ReferenceListViewModel
    {
        public override string Title => "All References";

        protected override IEnumerable<ReferenceModel> GetResults(AssemblyInformation information)
        {
            return information?.GetAllLinks()
                               .Distinct()
                               .Select(x => new ReferenceModel(x))
                               .OrderBy(x => x.Link.Assembly.Name);
        }
    }
}