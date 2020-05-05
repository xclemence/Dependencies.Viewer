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
    public class FileImportAssembly : IImportAssembly
    {
        public string Name => "File";
        public bool IsReady => true;

        public Task<AssemblyExchangeContent> ImportAsync(Func<UserControl, IExchangeViewModel<AssemblyExchangeContent>, Task<AssemblyExchangeContent>> _)
        {
            var openFileDialog = new OpenFileDialog()
            {
                DefaultExt = ".json",
                Filter = "JavaScript Object Notation (.json)|*.json"
            };

            var result = openFileDialog.ShowDialog();

            if (!(result ?? false))
                return Task.FromResult<AssemblyExchangeContent>(default);

            var serializeObject = System.IO.File.ReadAllText(openFileDialog.FileName);
            var model = JsonConvert.DeserializeObject<ExportModel>(serializeObject);

            return Task.FromResult(new AssemblyExchangeContent { Assembly = model.Assembly, Dependencies = model.Dependencies });
        }
    }
}
