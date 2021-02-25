using System.Collections.Generic;
using System.Threading.Tasks;
using Dependencies.Check.Models;

namespace Dependencies.Check.Interfaces
{
    public interface ICircularReferenceCheck
    {
        Task<IList<CircularReferenceError>> AnalyseAsync(string entry, IDictionary<string, AssemblyCheck> context);
    }
}