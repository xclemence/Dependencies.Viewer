using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Commands;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public class ReferencesGridViewModel : ObservableObject, IReferencesDetailsViewModel
    {
        private ICollectionView? filteredReferences;
        private AssemblyModel? assembly;
        private IEnumerable<ReferenceModel>? displayResults;

        private readonly CheckCommand checkCommand;
        private readonly OpenCommand openCommand;

        public ReferencesGridViewModel(FilterModel filter, CheckCommand checkCommand, OpenCommand openCommand)
        {
            this.checkCommand = checkCommand;
            this.openCommand = openCommand;

            OpenSubResultCommand = new Command<ReferenceModel>(x => this.openCommand.OpenSubAssembly(x.LoadedAssembly));
            OpenParentReferenceCommand = new Command<ReferenceModel>(async (x) => await this.openCommand.ViewParentReferenceAsync(Assembly, x).ConfigureAwait(false));

            CircularDependenciesCheckCommand = new Command<ReferenceModel>(async (x) => await this.checkCommand.CircularDependenciesCheck(x.LoadedAssembly).ConfigureAwait(false));
            MissingentryPointCheckCommand = new Command<ReferenceModel>(async (x) => await this.checkCommand.EntryPointNotFoundCheck(x.LoadedAssembly).ConfigureAwait(false));

            Filter = filter;
        }

        public FilterModel Filter { get; }

        public ICommand OpenSubResultCommand { get; }
        public ICommand OpenParentReferenceCommand { get; }
        public ICommand CircularDependenciesCheckCommand { get; }
        public ICommand MissingentryPointCheckCommand { get; }

        public AssemblyModel? Assembly
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

        public ICollectionView? FilteredReferences
        {
            get => filteredReferences;
            private set => Set(ref filteredReferences, value);
        }

        public void RefreshFilteredItems() => new Action(() => filteredReferences?.Refresh()).InvokeUiThread();

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

        private static IEnumerable<ReferenceModel>? GetResults(AssemblyModel? assembly) =>
            assembly?.ReferenceProvider.Select(x => x.Value).OrderBy(x => x.AssemblyFullName);
    }
}