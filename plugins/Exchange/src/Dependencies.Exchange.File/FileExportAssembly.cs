using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        public string Name => "File";
        public bool IsReady => true;

        public Task ExportAsync(AssemblyExchange assembly,
                                IList<AssemblyExchange> dependencies, 
                                Func<UserControl, IExchangeViewModel<AssemblyExchangeContent>, Task<AssemblyExchangeContent>> _)
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

            var serializeObject = JsonConvert.SerializeObject(new ExportModel { Assembly = assembly, Dependencies = dependencies }, Formatting.Indented );

            System.IO.File.WriteAllText(saveFileDialog.FileName, serializeObject);

            return Task.CompletedTask;
        }
    }
}
