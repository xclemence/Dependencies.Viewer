using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.Extensions;

public static class AssemblyInformationExtensions
{
    public static AssemblyModel ToAssemblyModel(this AssemblyInformation assembly, IEnumerable<AssemblyLink> links)
    {
        var referenceProvider = new Dictionary<string, ReferenceModel>();
        var references = links.Select(x => x.ToReferenceModel(referenceProvider)).ToList();

        foreach (var item in references)
            referenceProvider.Add(item.AssemblyFullName, item);

        return assembly.ToAssemblyModel(referenceProvider);
    }

    public static AssemblyModel ToAssemblyModel(this AssemblyInformation assembly, IReadOnlyDictionary<string, ReferenceModel> referenceProvider) =>
        new(assembly.Name, assembly.Links.Select(x => x.LinkFullName).ToImmutableList(), referenceProvider)
        {
            AssemblyName = assembly.AssemblyName,
            CreationDate = assembly.CreationDate,
            Creator = assembly.Creator,
            FilePath = assembly.FilePath,
            HasEntryPoint = assembly.HasEntryPoint,
            IsDebug = assembly.IsDebug,
            IsILOnly = assembly.IsILOnly,
            IsLocalAssembly = assembly.IsLocalAssembly,
            IsNative = assembly.IsNative,
            IsResolved = assembly.IsResolved,
            TargetFramework = assembly.TargetFramework,
            TargetProcessor = assembly.TargetProcessor?.ToString(),
            Version = assembly.LoadedVersion,
            ParentLinkNames = assembly.ParentLinkName.ToHashSet(),
        };

    private static ReferenceModel ToReferenceModel(this AssemblyLink link, IReadOnlyDictionary<string, ReferenceModel> referenceProvider) =>
        new(link.LinkFullName, link.Assembly.ToAssemblyModel(referenceProvider))
        {
            AssemblyVersion = link.LinkVersion
        };
}
