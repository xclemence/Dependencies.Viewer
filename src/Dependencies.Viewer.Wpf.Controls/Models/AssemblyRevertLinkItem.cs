using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dependencies.Viewer.Wpf.Controls.Base;

namespace Dependencies.Viewer.Wpf.Controls.Models
{
    [DebuggerDisplay("Assembly = {Assembly.Name}, Version = {Assembly.Version}, Parents count = {Parents.Count}")]
    public class AssemblyRevertLinkItem: ObservableObject
    {
        private bool isExpanded;
        private readonly Func<string, AssemblyModel> assemblyProvider;

        public AssemblyRevertLinkItem(AssemblyModel assembly, Func<string, AssemblyModel> assemblyProvider, bool loadParent = false)
        {
            Assembly = assembly;
            this.assemblyProvider = assemblyProvider;

            if (loadParent)
                Parents = Assembly.ParentLinkNames.Select(x => new AssemblyRevertLinkItem(assemblyProvider(x), this.assemblyProvider)).ToList();
        }

        public AssemblyModel Assembly { get; }

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (Set(ref isExpanded, value))
                    LoadSubCollection();
            }
        }

        public IList<AssemblyRevertLinkItem>? Parents { get; private set; }

        private void LoadSubCollection()
        {
            if (Parents is null)
                return;

            foreach (var item in Parents.Where(x => x.Parents == null))
            {
                item.Parents = item.Assembly.ParentLinkNames.Select(x => new AssemblyRevertLinkItem(assemblyProvider(x), assemblyProvider)).ToList();
            }
        }
    }
}
