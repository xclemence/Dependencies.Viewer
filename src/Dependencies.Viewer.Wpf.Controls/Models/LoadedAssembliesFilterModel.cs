using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class AssemblyTreeModel : ObservableObject
    {
        private bool isExpanded;

        public AssemblyTreeModel(ReferenceModel reference) => Reference = reference;

        public FilterCollection<AssemblyTreeModel> Collection { get; set; }

        public ReferenceModel Reference { get; }

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (Set(ref isExpanded, value))
                    TryLoadSubCollection(Collection);
            }
        }

        private void TryLoadSubCollection(IEnumerable<AssemblyTreeModel> references)
        {
            var predicate = Collection.Predicate;
            foreach (var item in references.Where(x => x.Collection == null))
            {
                var subItemS = item.Reference.LoadedAssembly.References.Select(r => new AssemblyTreeModel(r));
                item.Collection = new FilterCollection<AssemblyTreeModel>(subItemS, predicate, nameof(AssemblyTreeModel.AssemblyFullName));
            }
        }

        public string AssemblyFullName => Reference.AssemblyFullName;

        public override string ToString() => Reference.ToDisplayString();
    }

    public class FilterCollection<T> : List<T>
    {
        public FilterCollection(IEnumerable<T> collection, Predicate<object> predicate, string baseSortingProperty)
            : base(collection)
        {
            Predicate = predicate;
            FilteredItems = this?.GetCollectionView(predicate, baseSortingProperty);
        }

        public Predicate<object> Predicate { get; }

        public ICollectionView FilteredItems { get; private set; }

        public void RefreshFilter() => FilteredItems?.Refresh();
    }
}
