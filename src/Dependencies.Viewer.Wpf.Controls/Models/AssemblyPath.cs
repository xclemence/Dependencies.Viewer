using System.Collections.Generic;
using Dependencies.Analyser.Base.Models;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class AssemblyPath
    {
        public AssemblyInformation Assembly { get; set; }

        public bool IsRoot { get; set; }

        public IList<AssemblyPath> Parents { get; set; } = new List<AssemblyPath>();
    }
}
