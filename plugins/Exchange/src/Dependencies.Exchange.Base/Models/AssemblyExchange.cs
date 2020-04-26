using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dependencies.Exchange.Base.Models
{
    
    [DebuggerDisplay("Name = {ShortName}, Version = {Version}, FullName = {Name}")]
    public class AssemblyExchange
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public bool IsNative { get; set; }

        public string Version { get; set; }

        public string Creator { get; set; }

        public string TargetFramework { get; set; }

        public string TargetProcessor { get; set; }

        public bool? IsDebug { get; set; }

        public bool IsILOnly { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsPartial { get; set; }

        public bool IsLocal { get; set; }

        public List<string> AssembliesReferenced { get; set; } = new List<string>();
    }
}