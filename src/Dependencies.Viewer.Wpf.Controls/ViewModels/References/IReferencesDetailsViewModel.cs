using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public interface IReferencesDetailsViewModel
    {
        AssemblyModel Assembly { get; set; }
        FilterModel Filter { get; set; }
        void RefreshFilteredItems();
    }
}
