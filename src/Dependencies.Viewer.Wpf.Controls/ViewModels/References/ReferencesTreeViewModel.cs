using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public class ReferencesTreeViewModel : ObservableObject, IReferencesDetailsViewModel
    {
        private IEnumerable<AssemblyLinkModel> links;
        private AssemblyInformation assemblyInformation;
        private ICollectionView filteredLinks;

        public ReferencesTreeViewModel()
        {
            OpenSubResultCommand = new Command<AssemblyLinkModel>(x => GlobalCommand.OpenAssembly(x.Assembly));
            OpenParentReferenceCommand = new Command<AssemblyLinkModel>(async (x) => await GlobalCommand.ViewParentReferenceAsync(assemblyInformation, x.AssemblyLink).ConfigureAwait(false));
        }

        public ICommand OpenSubResultCommand { get; }
        public ICommand OpenParentReferenceCommand { get; }

        public AssemblyInformation AssemblyInformation
        {
            get => assemblyInformation;
            set
            {
                if (Set(ref assemblyInformation, value))
                    CreateFilteredCollection(value);
            }
        }

        public FilterModel Filter { get; set; }

        public ICollectionView FilteredLinks
        {
            get => filteredLinks;
            private set => Set(ref filteredLinks, value);
        }

        public void RefreshFilteredItems()
        {
            FilteredLinks.Refresh();

            foreach (var item in links)
                RefreshFilteredItems(item);
        }

        private void RefreshFilteredItems(AssemblyLinkModel link)
        {
            link.AssemblyModel.RefreshFilter();

            foreach (var item in link.AssemblyModel.Links)
                RefreshFilteredItems(item);
        }

        private void CreateFilteredCollection(AssemblyInformation value)
        {
            links = value?.ToViewModel(FilterPredicat).Links;

            FilteredLinks = CollectionViewSource.GetDefaultView(links);
            FilteredLinks.Filter = FilterPredicat;
            FilteredLinks.SortDescriptions.Add(new SortDescription(nameof(AssemblyLinkModel.AssemblyFullName), ListSortDirection.Ascending));
        }

        private bool FilterPredicat(object obj)
        {
            if (!(obj is AssemblyLinkModel link))
                return false;

            if (link.AssemblyModel.Links.Any(x => FilterPredicat(x)))
                return true;

            if (Filter.DisplayLocalOnly && !link.Assembly.IsLocalAssembly)
                return false;

            if (string.IsNullOrWhiteSpace(Filter.Name))
                return true;

            return link.Assembly.Name.Contains(Filter.Name, System.StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
