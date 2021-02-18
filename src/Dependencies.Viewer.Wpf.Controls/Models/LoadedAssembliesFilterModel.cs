using System.Collections.Generic;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class AssemblyTreeModel : ObservableObject
    {
        private bool isExpanded;

        public AssemblyTreeModel(ReferenceModel reference) => Reference = reference;

        public FilterCollection<AssemblyTreeModel>? Collection { get; set; }

        public ReferenceModel Reference { get; }

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (Set(ref isExpanded, value) && Collection is not null)
                    TryLoadSubCollection(Collection);
            }
        }

        private void TryLoadSubCollection(IEnumerable<AssemblyTreeModel> references)
        {
            if (Collection is null)
                return;
            
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

   
}
