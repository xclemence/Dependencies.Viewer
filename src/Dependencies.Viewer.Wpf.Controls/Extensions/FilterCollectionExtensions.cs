using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.Extensions
{
    public static class FilterCollectionExtensions
    {
        public static ICollectionView GetCollectionView<T>(this IEnumerable<T> models, Predicate<object> predicate, string sortingProperty = null)
        {
            var collectionView = CollectionViewSource.GetDefaultView(models);
            collectionView.Filter = predicate;
            
            if (!string.IsNullOrEmpty(sortingProperty))
                collectionView.SortDescriptions.Add(new SortDescription(sortingProperty, ListSortDirection.Ascending));

            return collectionView;
        }
    }
}
