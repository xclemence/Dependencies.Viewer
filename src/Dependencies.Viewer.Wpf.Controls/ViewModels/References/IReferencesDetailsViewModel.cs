using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public interface IReferencesDetailsViewModel
    {
        AssemblyInformation AssemblyInformation { get; set; }
        FilterModel Filter { get; set; }
        void RefreshFilteredItems();
    }
}
