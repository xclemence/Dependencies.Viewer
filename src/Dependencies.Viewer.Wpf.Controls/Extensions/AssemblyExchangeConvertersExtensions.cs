using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Dependencies.Exchange.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.Extensions
{
    public static class AssemblyExchangeConvertersExtensions
    {

        ////////////////////////////////////////////////////  To Exchange Model /////////////////////////////////////////////////////

        public static (AssemblyExchange assembly, IList<AssemblyExchange> dependencies) ToExchangeModel(this AssemblyModel assembly)
        {
            var assemblyExchange = assembly.ToExchange();

            var allReferences = assembly.ReferenceProvider.Select(x => x.Value).ToList();
            var loadedAssemblies = allReferences.Select(x => x.LoadedAssembly).Distinct().ToList();

            var dependencies = loadedAssemblies.Select(x => x.ToExchange()).ToList();

            dependencies.AddRange(allReferences.Where(x => x.IsMismatchVersion).Select(x => x.ToExchange()));

            return (assemblyExchange, dependencies);
        }

        private static AssemblyExchange ToExchange(this AssemblyModel assembly) => new AssemblyExchange
        {
            Name = assembly.FullName,
            ShortName = assembly.Name,
            Version = assembly.Version,
            TargetFramework = assembly.TargetFramework,
            TargetProcessor = assembly.TargetProcessor?.ToString(),
            IsDebug = assembly.IsDebug,
            IsILOnly = assembly.IsILOnly,
            IsLocal = assembly.IsLocalAssembly,
            IsNative = assembly.IsNative,
            Creator = assembly.Creator,
            CreationDate = assembly.CreationDate,
            HasEntryPoint = assembly.HasEntryPoint,
            IsPartial = !assembly.IsResolved,
            AssembliesReferenced = assembly.ReferencedAssemblyNames.ToList()
        };

        private static AssemblyExchange ToExchange(this ReferenceModel reference)
        {
            AssemblyExchange model;
            if (reference.IsMismatchVersion)
                model = new AssemblyExchange { Name = reference.AssemblyFullName, Version = reference.AssemblyVersion, IsPartial = true, ShortName = reference.LoadedAssembly.Name, IsLocal = reference.LoadedAssembly.IsLocalAssembly };
            else
                model = reference.LoadedAssembly.ToExchange();

            return model;
        }

        ////////////////////////////////////////////////////  To Assembly Model /////////////////////////////////////////////////////


        public static AssemblyModel ToAssemblyModel(this AssemblyExchange assemblyExchange, IList<AssemblyExchange> dependencies)
        {
            var referenceProvider = new Dictionary<string, ReferenceModel>();

            var loadedAssemblies = dependencies.GroupBy(x => x.ShortName).Select(x => GetLoadedItem(x)).ToList();

            var referenceCache = loadedAssemblies.Select(x => x.ToReferenceModelWithNewAssembly(referenceProvider)).ToDictionary(x => x.LoadedAssembly.Name);
            var notLoadedAssemblies = dependencies.Except(loadedAssemblies).Select(x => x.ToReferenceModelWithSearchAssembly(referenceCache));

            foreach (var (_, value) in referenceCache)
                referenceProvider.Add(value.AssemblyFullName, value);

            foreach (var item in notLoadedAssemblies)
                referenceProvider.Add(item.AssemblyFullName, item);

            var assembly = assemblyExchange.ToAssemblyModel(referenceProvider);

            assembly.ConsolidateMissingAssemblies(referenceProvider, referenceCache);

            return assembly;
        }

        private static void ConsolidateMissingAssemblies(this AssemblyModel assembly, Dictionary<string, ReferenceModel> referenceProvider, IReadOnlyDictionary<string, ReferenceModel> referenceCache)
        {
            var referencedAssemblies = referenceProvider.SelectMany(x => x.Value.LoadedAssembly.ReferencedAssemblyNames)
                                                        .Union(assembly.ReferencedAssemblyNames)
                                                        .Distinct()
                                                        .ToList();

            var referenceNotFound = referencedAssemblies.Where(x => !referenceProvider.Keys.Contains(x));

            foreach (var item in referenceNotFound)
            {
                var (referencedAssembly, assemblyName) = referenceCache.GetNotFoundAssembly(item, referenceProvider);

                referenceProvider.Add(item, new ReferenceModel
                {
                    AssemblyFullName = assemblyName.FullName,
                    AssemblyVersion = assemblyName.Version.ToString(),
                    LoadedAssembly = referencedAssembly
                });
            }
        }

        private static (AssemblyModel assembly, AssemblyName assemblyName) GetNotFoundAssembly(this IReadOnlyDictionary<string, ReferenceModel> referenceCache, string assemblyFullName, IReadOnlyDictionary<string, ReferenceModel> referenceProvider)
        {
            var assemblyName = new AssemblyName(assemblyFullName);

            if (referenceCache.TryGetValue(assemblyName.Name, out var reference))
                return (reference.LoadedAssembly, assemblyName);

            var assembly = assemblyName.ToNotFoundAssemblyModel(referenceProvider);

            return (assembly, assemblyName);
        }

        private static ReferenceModel ToReferenceModelWithNewAssembly(this AssemblyExchange assemblyExchange, IReadOnlyDictionary<string, ReferenceModel> referenceProvider) =>
            ToReferenceModelWithAssembly(assemblyExchange, assemblyExchange.ToAssemblyModel(referenceProvider));

        private static ReferenceModel ToReferenceModelWithSearchAssembly(this AssemblyExchange assemblyExchange, IReadOnlyDictionary<string, ReferenceModel> assemblyCache) =>
            ToReferenceModelWithAssembly(assemblyExchange, assemblyCache[assemblyExchange.ShortName].LoadedAssembly);

        public static ReferenceModel ToReferenceModelWithAssembly(this AssemblyExchange assemblyExchange, AssemblyModel assembly) => new ReferenceModel
        {
            AssemblyFullName = assemblyExchange.Name,
            AssemblyVersion = assemblyExchange.Version,
            LoadedAssembly = assembly
        };

        private static AssemblyExchange GetLoadedItem(IGrouping<string, AssemblyExchange> collection)
        {
            var item = collection.ToList();

            if (item.Count == 1) return item.First();

            return collection.OrderByDescending(x => new Version(x.Version)).First();
        }

        private static AssemblyModel ToAssemblyModel(this AssemblyExchange assembly, IReadOnlyDictionary<string, ReferenceModel> referenceProvider) => new AssemblyModel(referenceProvider)
        {
            Name = assembly.ShortName,
            Version = assembly.Version,
            AssemblyName = assembly.Name,
            TargetFramework = assembly.TargetFramework,
            TargetProcessor = assembly.TargetProcessor,
            IsDebug = assembly.IsDebug,
            IsILOnly = assembly.IsILOnly,
            IsLocalAssembly = assembly.IsLocal,
            IsNative = assembly.IsNative,
            Creator = assembly.Creator,
            CreationDate = assembly.CreationDate,
            HasEntryPoint = assembly.HasEntryPoint,
            IsResolved = !assembly.IsPartial,
            ReferencedAssemblyNames = assembly.AssembliesReferenced.ToImmutableList()
        };

        public static AssemblyModel ToNotFoundAssemblyModel(this AssemblyName assembly, IReadOnlyDictionary<string, ReferenceModel> referenceProvider) => new AssemblyModel(referenceProvider)
        {
            Name = assembly.Name,
            Version = assembly.Version?.ToString(),
            AssemblyName = assembly.FullName,
            IsResolved = false,
            ReferencedAssemblyNames = ImmutableList.Create<string>()
        };
    }
}
