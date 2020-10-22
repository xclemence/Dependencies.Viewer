using System;
using System.Collections.Generic;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Base;
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

        public static IList<AssemblyPathItem> GetAssemblyParentPath(this ReferenceModel reference, AssemblyModel assemblyRoot)
        {
            var cacheTransformer = new ObjectCacheTransformer();

            _ = GetAssemblyParentPath(reference, assemblyRoot, cacheTransformer).ToList();

            return cacheTransformer.GetCacheItems<string, (bool found, AssemblyPathItem pathItem)>().Where(x => x.found).Select(x => x.pathItem).ToArray();
        }

        private static IEnumerable<AssemblyPathItem> GetAssemblyParentPath(this ReferenceModel reference, AssemblyModel assemblyRoot, ObjectCacheTransformer cacheTransformer)
        {
            if (assemblyRoot.ReferencedAssemblyNames.Contains(reference.AssemblyFullName))
                yield return cacheTransformer.Transform(assemblyRoot.FullName, _ => (found: true, pathItem: new AssemblyPathItem { Assembly = assemblyRoot })).pathItem;

            var subPaths = assemblyRoot.References.SelectMany(x => reference.GetAssemblyParentPath(x.LoadedAssembly, cacheTransformer));

            var (found, pathItem) = cacheTransformer.Transform(assemblyRoot.FullName, _ => (found: false, pathItem: new AssemblyPathItem { Assembly = assemblyRoot }));

            foreach (var item in subPaths)
            {
                if (!item.Parents.Contains(pathItem))
                    item.Parents.Add(pathItem);
            }

            yield return pathItem;
        }

        public static AssemblyModel IsolatedShadowClone(this AssemblyModel assembly)
        {
            var referencedAssemblies = assembly.References.SelectMany(x => x.GetAllReferencedAssemblyNames()).Distinct().ToList();

            var limitedReferencesProvider = referencedAssemblies.Select(x => assembly.ReferenceProvider[x]).ToDictionary(x => x.AssemblyFullName, x => x.ShadowClone());

            foreach (var item in limitedReferencesProvider)
                item.Value.LoadedAssembly = item.Value.LoadedAssembly.ShadowClone(limitedReferencesProvider);

            return assembly.ShadowClone(limitedReferencesProvider);
        }

        public static IEnumerable<string> GetAllReferencedAssemblyNames(this ReferenceModel reference)
        {
            yield return reference.AssemblyFullName;

            foreach (var item in reference.LoadedAssembly.References.SelectMany(x => x.GetAllReferencedAssemblyNames()))
                yield return item;
        }
    }
}
