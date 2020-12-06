using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public class ReferencesViewModel : ObservableObject
    {
        private AssemblyModel? assembly;
        private readonly FilterModel filter;

        private readonly IReferencesDetailsViewModel[] resultViewModels;
        private IReferencesDetailsViewModel? selectedResultViewModels;

        public ReferencesViewModel()
        {
            filter = new FilterModel();

            resultViewModels = new IReferencesDetailsViewModel[]
            {
                new ReferencesTreeViewModel(filter),
                new ReferencesGridViewModel(filter)
            };

            SelectedResultViewModels = resultViewModels[0];
        }

        public AssemblyModel? Assembly
        {
            get => assembly;
            set
            {
                if (Set(ref assembly, value))
                {
                    foreach (var item in resultViewModels)
                        item.Assembly = value;
                }
            }
        }

        public IReferencesDetailsViewModel? SelectedResultViewModels
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
                SelectedResultViewModels?.RefreshFilteredItems();
            }
        }

        public string? NameFilter
        {
            get => filter.Name;
            set
            {
                if (filter.Name == value)
                    return;

                filter.Name = value;
                SelectedResultViewModels?.RefreshFilteredItems();
            }
        }
    }
}
