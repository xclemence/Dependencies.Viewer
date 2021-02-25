using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Dependencies.Check.Models;
using PeNet;

namespace Dependencies.Check
{
    public class MissingEntryPointCheck
    {
        private static MissingEntryPointError? FindMissingMethod(string baseName, string targetName, string? basePath, string? targetPath)
        {
            if (basePath == null || targetPath == null)
                return null;

            var importedFunction = new PeFile(basePath).ImportedFunctions;
            var exportedFunction = new PeFile(targetPath).ExportedFunctions;

            var importedErrors = importedFunction?.Where(x => x.DLL == targetName)
                                                  .Where(x => !(exportedFunction?.Any(y => y.Name == x.Name) ?? true))
                                                  .ToList();

            if (importedErrors == null || importedErrors.Count == 0)
                return null;

            return new MissingEntryPointError(baseName, targetName, importedErrors.Where(x => x.Name != null).Select(x => x.Name!).ToImmutableList());
        }

        public Task<IList<MissingEntryPointError>> AnalyseAsync(IDictionary<string, AssemblyCheck> assemblies) =>
            Task.Run(() => Analyse(assemblies).ToList() as IList<MissingEntryPointError>);

        private IEnumerable<MissingEntryPointError> Analyse(IDictionary<string, AssemblyCheck> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                foreach (var result in Analyse(assembly.Key, assemblies))
                    yield return result;
            }
        }

        private IEnumerable<MissingEntryPointError> Analyse(string assemblyName, IDictionary<string, AssemblyCheck> context)
        {
            var assembly = context[assemblyName];

            foreach(var child in assembly.AssembliesReferenced)
            {
                var childAssembly = context[child];

                if (childAssembly.IsLocal && (assembly.IsNative || childAssembly.IsNative))
                {
                    var errors = FindMissingMethod(assemblyName, child, assembly.Path, childAssembly.Path);
                    if (errors is not null) yield return errors;
                }
            }
        }

    }
}
