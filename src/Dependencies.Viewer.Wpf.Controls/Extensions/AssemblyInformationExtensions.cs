using System;
using System.Collections.Generic;
using System.Linq;
using Dependencies.Analyser.Base;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.Extensions
{
    public static class AssemblyInformationExtensions
    {

        public static string ToDisplayString(this AssemblyLink link, Func<AssemblyInformation, string> GetName)
        {
            if(!link.Assembly.IsResolved && !string.IsNullOrEmpty(link.LinkVersion))
                return $"{GetName(link.Assembly)} (v{link.LinkVersion})";

            if (!link.Assembly.IsResolved)
                return GetName(link.Assembly);

            if (link.LinkVersion != link.Assembly.LoadedVersion)
                return $"{GetName(link.Assembly)}   (v{ link.LinkVersion } ➜ v{ link.Assembly.LoadedVersion})";

            if (link.Assembly.IsNative)
                return $"{GetName(link.Assembly)}   (loaded v{ link.Assembly.LoadedVersion })";

            return GetName(link.Assembly);
        }

        public static string ToDisplayString(this AssemblyLink link) => link.ToDisplayString(x => x.FullName);

        public static int GetAllReferenceCount(this AssemblyInformation assembly) =>
          assembly.Links.Count + assembly.Links.Sum(x => x.Assembly.Links.Count);

        ///////////////////////////////////// Convert to model for view .///////////////////////////////////////////////////

        public static AssemblyInformationModel ToViewModel(this AssemblyInformation item, Predicate<object> predicate)
        {
            var cache = new ObjectCacheTransformer();
            return item.ToViewModel(predicate, cache);
        }

        public static AssemblyInformationModel ToViewModel(this AssemblyInformation baseItem, Predicate<object> predicate, ObjectCacheTransformer transformer)
        {
            var newItem = transformer.Transform(baseItem, x => new AssemblyInformationModel(x, predicate));

            newItem.Links = baseItem.Links?.Select(x => x.ToViewModel(predicate, transformer)).ToList();

            return newItem;
        }

        internal static AssemblyLinkModel ToViewModel(this AssemblyLink baseItem, Predicate<object> predicate, ObjectCacheTransformer transformer) =>
            new AssemblyLinkModel(transformer.Transform(baseItem, x => x.Assembly.ToViewModel(predicate, transformer)), baseItem);

        ///////////////////////////////////// Get parents path.///////////////////////////////////////////////////

        public static IEnumerable<AssemblyPath> GetAssemblyParentPath(this AssemblyInformation assembly, AssemblyLink link)
        {
            var cache = new ObjectCacheTransformer();
            assembly.GetAssemblyParentPath(link, cache).ToList();

            return cache.GetCacheItems<AssemblyInformation, AssemblyPath>().Where(x => x.IsRoot);
        }

        private static IEnumerable<AssemblyPath> GetAssemblyParentPath(this AssemblyInformation assembly, AssemblyLink researchLink, ObjectCacheTransformer cache)
        {
            if (assembly.Links.Contains(researchLink))
                yield return cache.Transform(assembly, x => new AssemblyPath { Assembly = x, IsRoot = true });

            var subPath = assembly.Links.SelectMany(x => x.Assembly.GetAssemblyParentPath(researchLink, cache)).ToList();

            if (subPath.Count != 0)
            {
                var currentPath = cache.Transform(assembly, x => new AssemblyPath { Assembly = x });

                foreach (var item in subPath)
                {
                    if (!item.Parents.Contains(currentPath))
                        item.Parents.Add(currentPath);
                }

                yield return currentPath;
            }
        }
    }
}
