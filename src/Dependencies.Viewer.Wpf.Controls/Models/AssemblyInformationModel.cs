using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using GalaSoft.MvvmLight;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class AssemblyInformationModel : ObservableObject
    {
        public bool isVisible;
        private IList<AssemblyLinkModel> links;
        private readonly Predicate<object> predicate;

        public AssemblyInformationModel(AssemblyInformation assemblyInformation, Predicate<object> predicate)
        {
            AssemblyInformation = assemblyInformation ?? throw new ArgumentNullException(nameof(assemblyInformation));
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            IsVisible = true;
        }

        public bool IsVisible
        {
            get => isVisible;
            set => Set(ref isVisible, value);
        }

        public AssemblyInformation AssemblyInformation { get; }

        public IList<AssemblyLinkModel> Links
        {
            get => links;
            set
            {
                if (Set(ref links, value))
                    FilteredLinks = value?.GetCollectionView(predicate);
            }
        }

        public ICollectionView FilteredLinks { get; private set; }

        public void RefreshFilter() => FilteredLinks?.Refresh();
    }
}
