using System.Collections.Generic;
using Dependencies.Viewer.Wpf.Controls.Models;

namespace Dependencies.Viewer.Wpf.Controls.ViewModels
{
    public class AssemblyParentsViewModel
    {
        public string BaseAssembly { get; set; }

        public IList<AssemblyPath> Paths { get; set; }
    }
}
