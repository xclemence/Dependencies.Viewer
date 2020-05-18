using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class AssemblyModel
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

        public AssemblyModel ShadowClone(IReadOnlyDictionary<string, ReferenceModel> referenceProvider)
        {
            var clone = (AssemblyModel)MemberwiseClone();

            clone.ReferenceProvider = referenceProvider;

            return clone;
        }
    }
}

