using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public class ReferencesGridViewModel : ObservableObject, IReferencesDetailsViewModel
    {
        private ICollectionView filteredReferences;
        private AssemblyModel assembly;
        private IEnumerable<ReferenceModel> displayResults;

        public ReferencesGridViewModel()
        {
            OpenSubResultCommand = new Command<ReferenceModel>(x => GlobalCommand.OpenAssembly(x.LoadedAssembly));
            OpenParentReferenceCommand = new Command<ReferenceModel>(async (x) => await GlobalCommand.ViewParentReferenceAsync(Assembly, x).ConfigureAwait(false));
        }

        public FilterModel Filter { get; set; }

        public ICommand OpenSubResultCommand { get; }
        public ICommand OpenParentReferenceCommand { get; }

        public AssemblyModel Assembly
        {
            get => assembly;
            set
            {
                if (Set(ref assembly, value))
                {
                    displayResults = GetResults(value);
                    CreateFilteredCollection();
                }
            }
        }

        public ICollectionView FilteredReferences
        {
            get => filteredReferences;
            private set => Set(ref filteredReferences, value);
        }

        public void RefreshFilteredItems() => new Action(() => filteredReferences.Refresh()).InvokeUiThread();

        private void CreateFilteredCollection()
        {
            new Action(() =>
            {
                FilteredReferences = CollectionViewSource.GetDefaultView(displayResults);
                FilteredReferences.Filter = FilterPredicate;
            }).InvokeUiThread();
        }

        private bool FilterPredicate(object obj)
        {
            if (obj is not ReferenceModel reference)
                return false;

            if (Filter.DisplayLocalOnly && !reference.LoadedAssembly.IsLocalAssembly)
                return false;

            if (string.IsNullOrWhiteSpace(Filter.Name))
                return true;

            if (reference.LoadedAssembly.Name.Contains(Filter.Name, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }

        private static IEnumerable<ReferenceModel> GetResults(AssemblyModel assembly) =>
            assembly?.ReferenceProvider.Select(x => x.Value).OrderBy(x => x.AssemblyFullName);
    }
}