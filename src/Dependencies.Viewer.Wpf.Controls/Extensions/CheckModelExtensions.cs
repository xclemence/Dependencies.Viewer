using System.Linq;
using Dependencies.Check.Model;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.Extensions
{
    internal static class CheckModelExtensions
    {
        public static AssemblyCheck ToCheckModel(this AssemblyModel assembly) => new AssemblyCheck
        {
            Name = assembly.Name,
            Version = assembly.Version ?? string.Empty,
            IsNative = assembly.IsNative,
            AssembliesReferenced = assembly.ReferencedAssemblyNames.Select(x => x.Split(",").First()).ToList()
        };
    }
}
