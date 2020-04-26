using System;
using Dependencies.Exchange.Base;

namespace Dependencies.Transfert.File
{
    public class FileAssemblyExchangeFactory : IAssemblyExchangeFactory
    {
        private readonly IExchangeServiceFactory<FileExportAssembly> exportServiceFractory;
        private readonly IExchangeServiceFactory<FileImportAssembly> importServiceFractory;

        public FileAssemblyExchangeFactory(IExchangeServiceFactory<FileExportAssembly> exportServiceFractory, IExchangeServiceFactory<FileImportAssembly> importServiceFractory)
        {
            this.exportServiceFractory = exportServiceFractory ?? throw new ArgumentNullException(nameof(exportServiceFractory));
            this.importServiceFractory = importServiceFractory ?? throw new ArgumentNullException(nameof(importServiceFractory));
        }

        public string Name => "File";

        public string Code => "File";

        public IExportAssembly GetExportService() => exportServiceFractory.Create();

        public IImportAssembly GetImportService() => importServiceFractory.Create();
    }
}
