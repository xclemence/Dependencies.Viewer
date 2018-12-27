using System;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class ReferenceModel
    {
        public ReferenceModel(AssemblyLink link)
        {
            Link = link ?? throw new ArgumentNullException(nameof(link));
        }

        public AssemblyLink Link { get; }

        public override string ToString() => Link.ToDisplayString(x => x.Name);
    }
}
