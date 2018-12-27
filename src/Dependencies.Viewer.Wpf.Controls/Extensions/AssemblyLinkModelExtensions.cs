using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.Extensions
{
    public static class AssemblyLinkModelExtensions
    {
        public static ICollectionView GetCollectionView(this IEnumerable<AssemblyLinkModel> links, Predicate<object> predicate)
        {
            var collectionView = CollectionViewSource.GetDefaultView(links);
            collectionView.Filter = predicate;
            collectionView.SortDescriptions.Add(new SortDescription(nameof(AssemblyLinkModel.AssemblyFullName), ListSortDirection.Ascending));

            return collectionView;
        }
    }
}
