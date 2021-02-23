using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Dependencies.Check.Model;

namespace Dependencies.Check
{
    public class CircularReferenceCheck
    {
        public IEnumerable<IImmutableList<string>> Analyse(string entry, IDictionary<string, AssemblyCheck> context) => 
            Analyse(entry, ImmutableList<string>.Empty, context);

        private IEnumerable<IImmutableList<string>> Analyse(string assemblyName, IImmutableList<string> parent, IDictionary<string, AssemblyCheck> context)
        {
            var hasCycle = parent.Contains(assemblyName);
            var currentPath = parent.Add(assemblyName);

            if (hasCycle) {
                yield return currentPath;
                yield break;
            }
            
            var assembly = context[assemblyName];

            foreach(var child in assembly.AssembliesReferenced)
            {
                foreach (var result in Analyse(child, currentPath, context).ToList())
                    yield return result;
            }
        }
    }
}
