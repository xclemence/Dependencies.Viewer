using System.Collections.Generic;
using System.Diagnostics;

namespace Dependencies.Check.Models
{
    [DebuggerDisplay("Name = {Name}, Version = {Version}")]
    public record AssemblyCheck
    {
        public string Name { get; init; } = string.Empty;

        public string Version { get; init; } = string.Empty;
        public string? Path { get; init; }

        public bool IsNative { get; init; }
        public bool IsLocal { get; init; }

        public List<string> AssembliesReferenced { get; init; } = new ();
    }
}