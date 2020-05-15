using System;
using Dependencies.Analyser.Base.Models;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Extensions;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class AssemblyLinkModel : ObservableObject
    {
        public AssemblyLinkModel(AssemblyInformationModel assemblyInformation, AssemblyLink assemblyLink)
        {
            AssemblyModel = assemblyInformation ?? throw new ArgumentNullException(nameof(assemblyInformation));
            AssemblyLink = assemblyLink ?? throw new ArgumentNullException(nameof(assemblyLink));
        }

        public AssemblyInformationModel AssemblyModel { get; }
        public AssemblyInformation Assembly => AssemblyModel.AssemblyInformation;

        public AssemblyLink AssemblyLink { get; }
        public bool IsExpanded { get; set; }

        public string AssemblyFullName => ToString();

        public override string ToString() => AssemblyLink.ToDisplayString();
    }
}
