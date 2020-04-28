using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
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
            IsPartial = false,
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
            var dependenciesCache = dependencies.Where(x => !x.IsPartial).Select(x => (target: x.ToInformation(), baseItem: x)).ToDictionary(x => x.baseItem.ShortName);
            var assemblyCache = dependencies.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => (assembly: x.First().ShortName, version: x.First().Version));
            
            var assembly = assemblyExchange.ToInformation();

            dependenciesCache.Add(assemblyExchange.ShortName, (assembly, assemblyExchange));

            foreach (var item in dependenciesCache)
                item.Value.target.AddLinkDependencies(item.Value.baseItem, dependenciesCache, assemblyCache);

            return assembly;
        }


        private static void AddLinkDependencies(this AssemblyInformation assembly,
                                                AssemblyExchange assemblyExchange,
                                                IDictionary<string, (AssemblyInformation target, AssemblyExchange baseItem)> assembliesCahes,
                                                IDictionary<string, (string assembly, string version)> assemblyExchangeCache)
        {
            // TODO Find a way to solve case where used assembly in not referenced.... (not found link ?)
            assembly.Links = assemblyExchange.AssembliesReferenced.Where(x => assembliesCahes.ContainsKey(assemblyExchangeCache[x].assembly)).Select(x => new AssemblyLink
            {
                Assembly = assembliesCahes[assemblyExchangeCache[x].assembly].target,
                LinkVersion = assemblyExchangeCache[x].version,
                LinkFullName = x
            }).ToList();
        }

        private static AssemblyInformation ToInformation(this AssemblyExchange assembly) => new AssemblyInformation
        {
            AssemblyName = assembly.Name,
            Name = assembly.ShortName,
            LoadedVersion = assembly.Version,
            TargetFramework = assembly.TargetFramework,
            TargetProcessor = assembly.TargetProcessor == null ? (TargetProcessor?) null : Enum.Parse<TargetProcessor>(assembly.TargetProcessor),
            IsDebug = assembly.IsDebug,
            IsILOnly = assembly.IsILOnly,
            IsLocalAssembly = assembly.IsLocal,
            IsNative = assembly.IsNative,
            Creator = assembly.Creator,
            CreationDate = assembly.CreationDate,
            Links = new List<AssemblyLink>(),
        };

    }
}
