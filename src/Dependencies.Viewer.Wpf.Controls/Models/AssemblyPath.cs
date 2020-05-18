using System.Collections.Generic;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    public class AssemblyPath
    {
        public AssemblyModel Assembly { get; set; }

        public bool IsRoot { get; set; }

        public IList<AssemblyPath> Parents { get; set; } = new List<AssemblyPath>();
    }
}
