using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels.References
{
    public class ReferencesTreeViewModel : ObservableObject, IReferencesDetailsViewModel
    {
        private AssemblyModel assembly;
        private FilterCollection<AssemblyTreeModel> loadedAssemblies;

        public ReferencesTreeViewModel()
        {
            OpenSubResultCommand = new Command<AssemblyTreeModel>(x => GlobalCommand.OpenAssembly(x.Reference.LoadedAssembly));
            OpenParentReferenceCommand = new Command<AssemblyTreeModel>(async (x) => await GlobalCommand.ViewParentReferenceAsync(assembly, x.Reference).ConfigureAwait(false));
        }

        public ICommand OpenSubResultCommand { get; }
        public ICommand OpenParentReferenceCommand { get; }

        public AssemblyModel Assembly
        {
            get => assembly;
            set
            {
                if (Set(ref assembly, value))
                    CreateFilteredCollection(value);
            }
        }

        public FilterCollection<AssemblyTreeModel> LoadedAssemblies
        {
            get => loadedAssemblies;
            set => Set(ref loadedAssemblies, value);
        }

        public FilterModel Filter { get; set; }

        public void RefreshFilteredItems()
        {
            LoadedAssemblies.RefreshFilter();

            RefreshFilteredItems(LoadedAssemblies);
        }

        private void RefreshFilteredItems(IEnumerable<AssemblyTreeModel> models)
        {
            foreach (var item in models)
            {
                item.Collection.RefreshFilter();

                RefreshFilteredItems(item.Collection);
            }
        }

        private void CreateFilteredCollection(AssemblyModel value) => LoadedAssemblies = value.References.ToFilterModels(FilterPredicate);

        private bool FilterPredicate(object obj)
        {
            if (!(obj is AssemblyTreeModel model))
                return false;

            if (Filter.DisplayLocalOnly && !model.Reference.LoadedAssembly.IsLocalAssembly)
                return false;

            if (string.IsNullOrWhiteSpace(Filter.Name))
                return true;

            if (model.Reference.AssemblyFullName.Contains(Filter.Name, System.StringComparison.InvariantCultureIgnoreCase))
                return true;

            return model.Collection.Any(x => FilterPredicate(x));
        }

    }
}
