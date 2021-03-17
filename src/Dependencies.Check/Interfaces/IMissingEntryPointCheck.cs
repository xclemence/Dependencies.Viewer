using System.Collections.Generic;
using System.Threading.Tasks;
using Dependencies.Check.Models;

namespace Dependencies.Check.Interfaces
{
    public interface IMissingEntryPointCheck
    {
        Task<IList<MissingEntryPointError>> AnalyseAsync(IDictionary<string, AssemblyCheck> assemblies);
    }
}