using System.Collections.Generic;
using System.Diagnostics;

namespace Dependencies.Check.Model
{
    [DebuggerDisplay("Name = {Name}, Version = {Version}")]
    public record AssemblyCheck
    {
        public string Name { get; init; } = string.Empty;

        public string Version { get; init; } = string.Empty;
        
        public bool IsNative { get; init; }

        public List<string> AssembliesReferenced { get; init; } = new ();
    }
}