using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Dependencies.Check.Models;

namespace Dependencies.Check
{
    public class CircularReferenceCheck
    {
        public Task<IList<CircularReferenceError>> AnalyseAsync(string entry, IDictionary<string, AssemblyCheck> context) =>
            Task.Run(() => Analyse(entry, ImmutableList<string>.Empty, context).ToList() as IList<CircularReferenceError>);

        private IEnumerable<CircularReferenceError> Analyse(string assemblyName, IImmutableList<string> parent, IDictionary<string, AssemblyCheck> context)
        {
            var hasCycle = parent.Contains(assemblyName);
            var currentPath = parent.Add(assemblyName);

            if (hasCycle) {
                yield return new CircularReferenceError(currentPath);
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
