using System;
using System.Collections.Generic;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.Extensions
{
    public static class ReferenceModelExtensions
    {
        public static string ToDisplayString(this ReferenceModel reference, Func<AssemblyModel, string> nameProvider)
        {
            if (!reference.LoadedAssembly.IsResolved && !string.IsNullOrEmpty(reference.AssemblyVersion))
                return $"{nameProvider(reference.LoadedAssembly)} (v{reference.AssemblyVersion})";

            if (!reference.LoadedAssembly.IsResolved)
                return nameProvider(reference.LoadedAssembly);

            if (reference.AssemblyVersion != reference.LoadedAssembly.Version)
                return $"{nameProvider(reference.LoadedAssembly)}   (v{ reference.AssemblyVersion } ➜ v{ reference.LoadedAssembly.Version})";

            if (reference.LoadedAssembly.IsNative)
                return $"{nameProvider(reference.LoadedAssembly)}   (loaded v{ reference.LoadedAssembly.Version })";

            return nameProvider(reference.LoadedAssembly);
        }

        public static string ToDisplayString(this ReferenceModel reference) => reference.ToDisplayString(x => x.FullName);

        public static FilterCollection<AssemblyTreeModel> ToFilterModels(this IEnumerable<ReferenceModel> references, Predicate<object> predicate)
        {
            var models = references.Select(x => {
                var subItem = x.LoadedAssembly.References.Select(r => new AssemblyTreeModel(r));

                var item = new AssemblyTreeModel(x)
                {
                    Collection = new FilterCollection<AssemblyTreeModel>(subItem, predicate, nameof(AssemblyTreeModel.AssemblyFullName))
                };

                return item;
            });

            return new FilterCollection<AssemblyTreeModel>(models, predicate, nameof(AssemblyTreeModel.AssemblyFullName));
        }

        public static AssemblyModel IsolatedShadowClone(this AssemblyModel assembly)
        {
            var referencedAssemblies = new HashSet<string>();

            foreach (var item in assembly.References)
                item.AppendReferencedAssemblyNames(referencedAssemblies);

            var limitedReferencesProvider = referencedAssemblies.Select(x => assembly.ReferenceProvider[x]).ToDictionary(x => x.AssemblyFullName, x => x.ShadowClone());

            foreach (var item in limitedReferencesProvider)
                item.Value.LoadedAssembly = item.Value.LoadedAssembly.ShadowClone(limitedReferencesProvider);

            var clone = assembly.ShadowClone(limitedReferencesProvider);

            clone.ParentLinkNames.Clear();

            limitedReferencesProvider.CleanNotExistingParentLink(clone.FullName);

            return clone;
        }

        public static void CleanNotExistingParentLink(this Dictionary<string, ReferenceModel> references, string rootName)
        {
            foreach(var item in references.Values)
            {
                item.LoadedAssembly.ParentLinkNames = item.LoadedAssembly.ParentLinkNames.Where(x => x == rootName || references.ContainsKey(x)).ToHashSet();
            }
        }

        public static void AppendReferencedAssemblyNames(this ReferenceModel reference, HashSet<string> includedReferences)
        {
            if (includedReferences.Contains(reference.AssemblyFullName))
                return;

            includedReferences.Add(reference.AssemblyFullName);

            foreach (var item in reference.LoadedAssembly.References)
                item.AppendReferencedAssemblyNames(includedReferences);
        }
    }
}
