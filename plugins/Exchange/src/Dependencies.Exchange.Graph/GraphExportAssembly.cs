using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;
using Dependencies.Exchange.Graph.Extensions;
using Newtonsoft.Json;

namespace Dependencies.Exchange.Graph
{
    public class GraphExportAssembly : IExportAssembly
    {
        
        private static readonly HttpClient client = new HttpClient();

        public async Task ExportAsync(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)
        {
            var assemblyDtos = dependencies.Where(x => x.IsLocal).Select(x => x.ToDto()).ToList();

            assemblyDtos.Add(assembly.ToDto());

            var json = JsonConvert.SerializeObject(assemblyDtos);
            using var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(@"http://localhost:32768/api/assembly/add", data);
            response.EnsureSuccessStatusCode();
        }
    }
}
