using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Dependencies.Analyser.Base.Extensions;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public class ReferencesGridViewModel : ViewModelBase, IReferencesDetailsViewModel
    {
        private ICollectionView filteredLinks;
        private AssemblyInformation assemblyInformation;
        private IEnumerable<ReferenceModel> displayResults;

        public ReferencesGridViewModel()
        {
            OpenSubResultCommand = new RelayCommand<ReferenceModel>(x => GlobalCommand.OpenAssembly(x.Link.Assembly));
            OpenParentReferenceCommand = new RelayCommand<ReferenceModel>(async (x) => await GlobalCommand.ViewParentReferenceAsync(AssemblyInformation, x.Link));
        }

        public FilterModel Filter { get; set; }

        public ICommand OpenSubResultCommand { get; }
        public ICommand OpenParentReferenceCommand { get; }

        public AssemblyInformation AssemblyInformation
        {
            get => assemblyInformation;
            set
            {
                if (Set(ref assemblyInformation, value))
                {
                    displayResults = GetResults(value);
                    CreateFilteredCollection();
                }
            }
        }

        public ICollectionView FilteredLinks
        {
            get => filteredLinks;
            private set => Set(ref filteredLinks, value);
        }

        public void RefreshFilteredItems()
        {
            new Action(() => filteredLinks.Refresh()).InvokeUiThread();
        }

        private void CreateFilteredCollection()
        {
            new Action(() =>
            {
                FilteredLinks = CollectionViewSource.GetDefaultView(displayResults);
                FilteredLinks.Filter = FilterPredicat;
            }).InvokeUiThread();
        }

        private bool FilterPredicat(object obj)
        {
            if (!(obj is ReferenceModel reference))
                return false;

            if (reference.Link.Assembly.Links.Any(x => FilterPredicat(x)))
                return true;

            if (Filter.DisplayLocalOnly && !reference.Link.Assembly.IsLocalAssembly)
                return false;

            if (string.IsNullOrWhiteSpace(Filter.Name))
                return true;

            return reference.Link.Assembly.Name.ToUpper().Contains(Filter.Name.ToUpper());
        }

        private IEnumerable<ReferenceModel> GetResults(AssemblyInformation information)
        {
            return information?.GetAllLinks()
                               .Distinct()
                               .Select(x => new ReferenceModel(x))
                               .OrderBy(x => x.Link.Assembly.Name);
        }
    }
}