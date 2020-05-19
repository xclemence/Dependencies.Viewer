using System.Collections.Generic;
using System.Diagnostics;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    [DebuggerDisplay("Assembly = {Assembly.Name}, Version = {Assembly.Version}, Parents count = {Parents.Count}")]
    public class AssemblyPathItem
    {
        public AssemblyModel Assembly { get; set; }

        public IList<AssemblyPathItem> Parents { get; set; } = new List<AssemblyPathItem>();
    }
}
