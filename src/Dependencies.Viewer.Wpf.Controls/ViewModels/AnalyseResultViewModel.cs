using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AnalyseResultViewModel : ViewModelBase
    {
        private IEnumerable<AssemblyLinkModel> links;
        private string nameFilter;
        private bool displayLocalOnly;
        private AssemblyInformation assemblyInformation;
        private ICollectionView filteredLinks;

        public AnalyseResultViewModel() =>
            OpenSubResultCommand = new RelayCommand<AssemblyLinkModel>(x => OpenSubResult?.Invoke(x.Assembly));

        public Action<AssemblyInformation> OpenSubResult { get; set; }

        public ICommand OpenSubResultCommand { get; }

        public AssemblyInformation AssemblyInformation
        {
            get => assemblyInformation;
            set
            {
                if (Set(ref assemblyInformation, value))
                {
                    CreateFilteredCollection(value);
                }
            }
        }

        private void CreateFilteredCollection(AssemblyInformation value)
        {
            links = value?.ToViewModel(FilterPredicat).Links;

            FilteredLinks = CollectionViewSource.GetDefaultView(links);
            FilteredLinks.Filter = FilterPredicat;
            FilteredLinks.SortDescriptions.Add(new SortDescription(nameof(AssemblyLinkModel.AssemblyFullName), ListSortDirection.Ascending));
        }

        public ICollectionView FilteredLinks
        {
            get => filteredLinks;
            private set => Set(ref filteredLinks, value);
        }


        public bool DisplayLocalOnly
        {
            get => displayLocalOnly;
            set
            {
                if (Set(ref displayLocalOnly, value))
                    RefreshFilteredItems(links);
            }
        }

        public string NameFilter
        {
            get => nameFilter;
            set
            {
                if (Set(ref nameFilter, value))
                    RefreshFilteredItems(links);
            }
        }

        private void RefreshFilteredItems(IEnumerable<AssemblyLinkModel> links)
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

        private bool FilterPredicat(object obj)
        {
            if (!(obj is AssemblyLinkModel link))
                return false;

            if (link.AssemblyModel.Links.Any(x => FilterPredicat(x)))
                return true;

            if (displayLocalOnly && !link.Assembly.IsLocalAssembly)
                return false;

            if (string.IsNullOrWhiteSpace(nameFilter))
                return true;

            return link.Assembly.Name.ToUpper().Contains(nameFilter.ToUpper());
        }
    }
}
