using System.Collections.Generic;
using System.Threading.Tasks;
using Dependencies.Exchange.Base;
using Dependencies.Exchange.Base.Models;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace Dependencies.Exchange.File
{
    class ExportModel 
    {
        public AssemblyExchange Assembly { get; set; }
        public IList<AssemblyExchange> Dependencies { get; set; }

    }

    public class FileExportAssembly : IExportAssembly
    {
        public Task ExportAsync(AssemblyExchange assembly, IList<AssemblyExchange> dependencies)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                FileName = assembly.ShortName,
                DefaultExt =".json",
                Filter = "JavaScript Object Notation (.json)|*.json"
            };

            var result = saveFileDialog.ShowDialog();

            if (!(result ?? false))
                return Task.CompletedTask;

            var serializeObject = JsonConvert.SerializeObject(new ExportModel { Assembly = assembly, Dependencies = dependencies } );

            System.IO.File.WriteAllText(saveFileDialog.FileName, serializeObject);

            return Task.CompletedTask;
        }
    }
}
