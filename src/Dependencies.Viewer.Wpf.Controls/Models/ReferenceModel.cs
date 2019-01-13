using System;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public enum AssemblyType
    {
        Managed,
        Native,
        System,
        Unknown
    }

    public class ReferenceModel
    {
        private int? allReferencesCount;

        public ReferenceModel(AssemblyLink link)
        {
            Link = link ?? throw new ArgumentNullException(nameof(link));
        }

        public int? AllReferencesCount
        {
            get
            {
                if (!allReferencesCount.HasValue)
                    allReferencesCount = Link.Assembly.GetAllReferenceCount();

                return allReferencesCount;
            }
        }

        public AssemblyType AssemblyType
        {
            get
            {
                if (!Link.Assembly.IsResolved)
                    return AssemblyType.Unknown;

                if (!Link.Assembly.IsLocalAssembly)
                    return AssemblyType.System;

                return Link.Assembly.IsNative ? AssemblyType.Native : AssemblyType.Managed;
            }
        }

        public bool IsMismatchVersion => Link.LinkVersion != Link.Assembly.LoadedVersion;

        public AssemblyLink Link { get; }

        public override string ToString() => Link.ToDisplayString(x => x.Name);
    }
}

