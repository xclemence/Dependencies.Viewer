using System;
using System.Collections.Generic;
using System.ComponentModel;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class AssemblyTreeModel : ObservableObject
    {
        public AssemblyTreeModel(ReferenceModel reference)
        {
            Reference = reference;
        }

        public FilterCollection<AssemblyTreeModel> Collection { get; set; }

        public ReferenceModel Reference { get; }

        public bool IsExpanded { get; set; }

        public string AssemblyFullName => Reference.AssemblyFullName;

        public override string ToString() => Reference.ToDisplayString();
    }

    public class FilterCollection<T> : List<T>
    {
        public FilterCollection(IEnumerable<T> collection, Predicate<object> predicate, string baseSortingProperty)
            : base(collection)
        {
            FilteredItems = this?.GetCollectionView(predicate, baseSortingProperty);
        }

        public ICollectionView FilteredItems { get; private set; }

        public void RefreshFilter() => FilteredItems?.Refresh();
    }
}
