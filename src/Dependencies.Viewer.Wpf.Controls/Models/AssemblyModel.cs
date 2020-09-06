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
        public AssemblyModel(IReadOnlyDictionary<string, ReferenceModel> referenceProvider) =>
            ReferenceProvider = referenceProvider ?? throw new ArgumentNullException(nameof(referenceProvider));

        public IReadOnlyDictionary<string, ReferenceModel> ReferenceProvider { get; private set; }

        public string Name { get; set; }
        public string Version { get; set; }

        public string AssemblyName { get; set; }

        public bool IsLocalAssembly { get; set; }

        public bool IsNative { get; set; }

        public bool IsResolved { get; set; } = true;

        public string FullName => AssemblyName ?? Name;

        public string FilePath { get; set; }

        public bool? IsDebug { get; set; }

        public bool IsILOnly { get; set; }

        public string TargetFramework { get; set; }

        public bool HasEntryPoint { get; set; }

        public string TargetProcessor { get; set; }

        public string Creator { get; set; }

        public DateTime CreationDate { get; set; }

        public IImmutableList<string> ReferencedAssemblyNames { get; set; }

        public IImmutableList<ReferenceModel> References => ReferencedAssemblyNames.Select(x => ReferenceProvider[x]).ToImmutableList();

        public override bool Equals(object obj) => Equals(obj as AssemblyModel);
        public bool Equals(AssemblyModel other) => other != null &&
                                                   FullName == other.FullName &&
                                                   IsDebug == other.IsDebug &&
                                                   TargetFramework == other.TargetFramework &&
                                                   TargetProcessor == other.TargetProcessor;
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

