using System.Diagnostics;
using Dependencies.Viewer.Wpf.Controls.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    [DebuggerDisplay("Assembly = {LoadedAssembly.Name}, Ref Version = {AssemblyVersion}, Loaded version = {LoadedAssembly.Version}")]
    public class ReferenceModel
    {

        public ReferenceModel(string assemblyFullName, AssemblyModel loadedAssembly)
        {
            AssemblyFullName = assemblyFullName;
            LoadedAssembly = loadedAssembly;
        }
        public string? AssemblyVersion { get; init; }

        public string AssemblyFullName { get; }

        public AssemblyModel LoadedAssembly { get; set; }

        public AssemblyType AssemblyType
        {
            get
            {
                if (!LoadedAssembly.IsResolved)
                    return AssemblyType.Unknown;

                if (!LoadedAssembly.IsLocalAssembly)
                    return AssemblyType.System;

                return LoadedAssembly.IsNative ? AssemblyType.Native : AssemblyType.Managed;
            }
        }

        public bool IsMismatchVersion => AssemblyVersion != LoadedAssembly.Version;

        public override string ToString() => this.ToDisplayString(x => x.Name);

        public ReferenceModel ShadowClone() => (ReferenceModel)MemberwiseClone();

    }

    public enum AssemblyType
    {
        Managed,
        Native,
        System,
        Unknown
    }
}
