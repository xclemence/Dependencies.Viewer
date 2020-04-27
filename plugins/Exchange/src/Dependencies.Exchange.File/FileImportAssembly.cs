using System.Collections.Generic;
using System.Threading.Tasks;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Dependencies.Exchange.File
{
    public class FileImportAssembly : IImportAssembly
    {
        public Task<(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)> ImportAsync()
        {
            var openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".json",
                Filter = "JavaScript Object Notation (.json)|*.json"
            };

            var result = openFileDialog.ShowDialog();

            if (!(result ?? false))
                return Task.FromResult<(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)>(default);

            var serializeObject = System.IO.File.ReadAllText(openFileDialog.FileName);
            var model = JsonConvert.DeserializeObject<ExportModel>(serializeObject);

            return Task.FromResult((model.Assembly, model.Dependencies));
        }

    }
}
