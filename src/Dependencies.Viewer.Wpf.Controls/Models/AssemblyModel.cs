using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    [DebuggerDisplay("Assembly = {Name}, Version = {Version}")]
    public sealed class AssemblyModel : IEquatable<AssemblyModel>
    {
        public AssemblyModel(string name, IImmutableList<string> referencedAssemblyNames, IReadOnlyDictionary<string, ReferenceModel> referenceProvider)
        {
            Name = name;
            ReferencedAssemblyNames = referencedAssemblyNames;
            ReferenceProvider = referenceProvider;
        }

        public IReadOnlyDictionary<string, ReferenceModel> ReferenceProvider { get; private set; }

        public string Name { get; }
        public string? Version { get; init; }

        public string? AssemblyName { get; init; }

        public bool IsLocalAssembly { get; init; }

        public bool IsNative { get; init; }

        public bool IsResolved { get; init; } = true;

        public string FullName => AssemblyName ?? Name;

        public string? FilePath { get; init; }

        public bool? IsDebug { get; init; }

        public bool IsILOnly { get; init; }

        public string? TargetFramework { get; init; }

        public bool HasEntryPoint { get; init; }

        public string? TargetProcessor { get; init; }

        public string? Creator { get; init; }

        public DateTime CreationDate { get; init; }

        public IImmutableList<string> ReferencedAssemblyNames { get; }

        public IImmutableList<ReferenceModel> References => ReferencedAssemblyNames.Select(x => ReferenceProvider[x]).ToImmutableList();

        public HashSet<string> ParentLinkNames { get; set; } = new HashSet<string>();

        public bool Equals(AssemblyModel? other)
        {
            if (other is null)
                return false;

            return FullName == other.FullName &&
                   IsDebug == other.IsDebug &&
                   TargetFramework == other.TargetFramework &&
                   TargetProcessor == other.TargetProcessor;
        }

        public override bool Equals(object? obj) => Equals(obj as AssemblyModel);

        public override int GetHashCode() => HashCode.Combine(FullName, IsDebug, TargetFramework, TargetProcessor);

        public AssemblyModel ShadowClone(IReadOnlyDictionary<string, ReferenceModel> referenceProvider)
        {
            var clone = (AssemblyModel)MemberwiseClone();

            clone.ReferenceProvider = referenceProvider;

            return clone;
        }

        public static bool operator ==(AssemblyModel left, AssemblyModel right) => EqualityComparer<AssemblyModel>.Default.Equals(left, right);
        public static bool operator !=(AssemblyModel left, AssemblyModel right) => !(left == right);

    }
}

