using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dependencies.Exchange.Base.Models;
using Dependencies.Exchange.Graph.Extensions;
using Newtonsoft.Json;

namespace Dependencies.Exchange.Graph
{
    public class AssemblyGraphService
    {
        private static readonly HttpClient client = new HttpClient();

        private const string ClientUrl = "http://localhost:5001";

        public async Task AddAsync(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)
        {
            var assemblyDtos = dependencies.Select(x => x.ToDto()).ToList();

            assemblyDtos.Add(assembly.ToDto());

            var json = JsonConvert.SerializeObject(assemblyDtos);
            using var data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(@$"{ClientUrl}/api/assembly/add", data);

            response.EnsureSuccessStatusCode();
        }

        public async Task<IList<string>> SearchAsync(string name)
        {
            HttpResponseMessage response = await client.GetAsync(@$"{ClientUrl}/api/assembly/search/{name}");
            response.EnsureSuccessStatusCode();

            var restult = response.Content;

            var result = JsonConvert.DeserializeObject<List<AssemblyDto>>(await response.Content.ReadAsStringAsync());

            return result.Select(x => x.Name).ToList();
        }

        public async Task<(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)> GetAsync(string name)
        {
            HttpResponseMessage response = await client.GetAsync(@$"{ClientUrl}/api/assembly/{name}");
            response.EnsureSuccessStatusCode();

            var restult = response.Content;

            var result = JsonConvert.DeserializeObject<List<AssemblyDto>>(await response.Content.ReadAsStringAsync());

            var assemblies = result.Select(x => x.ToExchange()).ToList();

            var mainAssembly = assemblies.First(x => x.Name == name);

            assemblies.Remove(mainAssembly);

            return (mainAssembly, assemblies);
        }
    }
}
