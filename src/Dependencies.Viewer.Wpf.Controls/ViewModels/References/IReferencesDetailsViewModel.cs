using System.Windows.Input;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public interface IReferencesDetailsViewModel
    {
        ICommand CircularDependenciesCheckCommand { get; }
        ICommand OpenSubResultCommand { get; }
        ICommand OpenParentReferenceCommand { get; }
        ICommand MissingentryPointCheckCommand { get; }

        AssemblyModel? Assembly { get; set; }
        FilterModel Filter { get; }
        void RefreshFilteredItems();
    }
}
