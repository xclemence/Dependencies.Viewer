using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Navigation;
using Dependencies.Analyser.Base.Extensions;
using Dependencies.Analyser.Base.Models;
using Dependencies.Exchange.Base.Models;

namespace Dependencies.Viewer.Wpf.Controls.Extensions
{
    public static class ImportExportConverters
    {
        public static (AssemblyExchange assembly, IList<AssemblyExchange> dependencies) ToExchangeModel(this AssemblyInformation assemblyInformation)
        {
            var assembly = assemblyInformation.ToExchange();

            var allLink = assemblyInformation.GetAllLinks().Distinct().ToList();
            var assemblies = allLink.Select(x => x.Assembly).Distinct().ToList();

            var dependencies = assemblies.Select(x => x.ToExchange()).ToList();

            dependencies.AddRange(allLink.Where(x => x.LinkVersion != x.Assembly.LoadedVersion).Select(x => x.ToExchange()));

            return (assembly, dependencies);
        }

        private static AssemblyExchange ToExchange(this AssemblyInformation assembly) => new AssemblyExchange
        {
            Name = assembly.FullName,
            ShortName = assembly.Name,
            Version = assembly.LoadedVersion,
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
            AssembliesReferenced = assembly.Links.Select(x => x.LinkFullName).ToList()
        };

        private static AssemblyExchange ToExchange(this AssemblyLink link) 
        {
            AssemblyExchange model;
            if (link.LinkVersion != link.Assembly.LoadedVersion)
                model = new AssemblyExchange { Name = link.LinkFullName, Version = link.LinkVersion, IsPartial = true, ShortName = link.Assembly.Name, IsLocal = link.Assembly.IsLocalAssembly };
            else
                model = link.Assembly.ToExchange();
            
            return model;
        }

        public static AssemblyInformation ToInformationModel(this AssemblyExchange assemblyExchange, IList<AssemblyExchange> dependencies)
        {
            var dependenciesCache = dependencies.GroupBy(x => x.ShortName)
                                                .Select(x => GetLoadedItem(x))
                                                .Select(x => (target: x.ToInformation(), baseItem: x)).ToDictionary(x => x.baseItem.ShortName);

            var assemblyCache = dependencies.ToDictionary(x => x.Name, x => x);

            var assembly = assemblyExchange.ToInformation();

            dependenciesCache.Add(assemblyExchange.ShortName, (assembly, assemblyExchange));

            foreach (var item in dependenciesCache)
                item.Value.target.AddLinkDependencies(item.Value.baseItem, dependenciesCache, assemblyCache);

            return assembly;
        }

        private static AssemblyExchange GetLoadedItem(IGrouping<string, AssemblyExchange> collection)
        {
            var item = collection.ToList();

            if (item.Count == 1) return item.First();

            return collection.OrderByDescending(x => new Version(x.Version)).First();
        }

        private static void AddLinkDependencies(this AssemblyInformation assembly,
                                                AssemblyExchange assemblyExchange,
                                                IDictionary<string, (AssemblyInformation target, AssemblyExchange baseItem)> assembliesCahes,
                                                IDictionary<string, AssemblyExchange> assemblyExchangeCache)
        {
            assembly.Links = assemblyExchange.AssembliesReferenced.Select(x => GetAsseblyLinkFromCache(assembliesCahes, assemblyExchangeCache, x)).ToList();
        }

        private static AssemblyLink GetAsseblyLinkFromCache(IDictionary<string, (AssemblyInformation target, AssemblyExchange baseItem)> assembliesCahes, 
                                                            IDictionary<string, AssemblyExchange> assemblyExchangeCache,
                                                            string x)
        { 
            var assembly = assemblyExchangeCache[x];

            if (assembliesCahes.ContainsKey(assembly.ShortName))
            {
                return new AssemblyLink
                {
                    Assembly = assembliesCahes[assembly.ShortName].target,
                    LinkVersion = assemblyExchangeCache[x].Version,
                    LinkFullName = x
                };
            }

            return new AssemblyLink
            {
                Assembly = assembly.ToInformation(),
                LinkVersion = assembly.Version,
                LinkFullName = x
            };
        }

        private static AssemblyInformation ToInformation(this AssemblyExchange assembly) => new AssemblyInformation(assembly.ShortName, assembly.Version, null)
        {
            AssemblyName = assembly.Name,
            TargetFramework = assembly.TargetFramework,
            TargetProcessor = assembly.TargetProcessor == null ? (TargetProcessor?) null : Enum.Parse<TargetProcessor>(assembly.TargetProcessor),
            IsDebug = assembly.IsDebug,
            IsILOnly = assembly.IsILOnly,
            IsLocalAssembly = assembly.IsLocal,
            IsNative = assembly.IsNative,
            Creator = assembly.Creator,
            CreationDate = assembly.CreationDate,
            HasEntryPoint = assembly.HasEntryPoint,
            IsResolved = !assembly.IsPartial
        };

    }
}
