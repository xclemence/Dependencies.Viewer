using System;
using Dependencies.Viewer.Wpf.Controls.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class ReferenceModel
    {
        public string AssemblyVersion { get; set; }

        public string AssemblyFullName { get; set; }

        public AssemblyModel LoadedAssembly { get; set; }

        public int? AllReferencesCount { get; set; }

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
