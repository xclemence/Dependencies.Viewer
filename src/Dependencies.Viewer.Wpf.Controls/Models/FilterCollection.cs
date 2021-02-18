using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dependencies.Viewer.Wpf.Controls.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class FilterCollection<T> : List<T>
    {
        public FilterCollection(IEnumerable<T> collection, Predicate<object> predicate, string baseSortingProperty)
            : base(collection)
        {
            Predicate = predicate;
            FilteredItems = this.GetCollectionView(predicate, baseSortingProperty);
        }

        public Predicate<object> Predicate { get; }

        public ICollectionView FilteredItems { get; private set; }

        public void RefreshFilter() => FilteredItems?.Refresh();
    }
}
