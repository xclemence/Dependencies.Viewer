using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Models;
using GalaSoft.MvvmLight;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public class ReferencesViewModel : ViewModelBase
    {
        private AssemblyInformation assemblyInformation;
        private FilterModel filter;

        private IReferencesDetailsViewModel[] resultViewModels;
        private IReferencesDetailsViewModel selectedResultViewModels;

        public ReferencesViewModel()
        {
            filter = new FilterModel();

            resultViewModels = new IReferencesDetailsViewModel[]
            {
                new ReferencesTreeViewModel { Filter = filter },
                new ReferencesGridViewModel { Filter = filter }
            };

            SelectedResultViewModels = resultViewModels[0];
        }

        public AssemblyInformation AssemblyInformation
        {
            get => assemblyInformation;
            set
            {
                if (Set(ref assemblyInformation, value))
                {
                    foreach (var item in resultViewModels)
                        item.AssemblyInformation = value;
                }
            }
        }

        public IReferencesDetailsViewModel SelectedResultViewModels
        {
            get => selectedResultViewModels;
            private set => Set(ref selectedResultViewModels, value);
        }

        public bool IsTreeMode
        {
            get => selectedResultViewModels == resultViewModels[0];
            set
            {
                var resultIndex = value ? 0 : 1;

                if (selectedResultViewModels == resultViewModels[resultIndex])
                    return;

                SelectedResultViewModels = resultViewModels[resultIndex];
                SelectedResultViewModels.RefreshFilteredItems();
            }
        }

        public bool DisplayLocalOnly
        {
            get => filter.DisplayLocalOnly;
            set
            {
                if (filter.DisplayLocalOnly == value)
                    return;

                filter.DisplayLocalOnly = value;
                SelectedResultViewModels.RefreshFilteredItems();
            }
        }

        public string NameFilter
        {
            get => filter.Name;
            set
            {
                if (filter.Name == value)
                    return;

                filter.Name = value;
                SelectedResultViewModels.RefreshFilteredItems();
            }
        }
    }
}
