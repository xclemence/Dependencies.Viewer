using System;
using Dependencies.Exchange.Base;

namespace Dependencies.Exchange.Graph
{
    public class GraphAssemblyExchangeFactory : IAssemblyExchangeFactory
    {
        private readonly IExchangeServiceFactory<GraphExportAssembly> exportServiceFractory;
        private readonly IExchangeServiceFactory<GraphImportAssembly> importServiceFractory;

        public GraphAssemblyExchangeFactory(IExchangeServiceFactory<GraphExportAssembly> exportServiceFractory, IExchangeServiceFactory<GraphImportAssembly> importServiceFractory)
        {
            this.exportServiceFractory = exportServiceFractory ?? throw new ArgumentNullException(nameof(exportServiceFractory));
            this.importServiceFractory = importServiceFractory ?? throw new ArgumentNullException(nameof(importServiceFractory));
        }

        public string Name => "Graph";

        public string Code => "Graph";

        public IExportAssembly GetExportService() => exportServiceFractory.Create();

        public IImportAssembly GetImportService() => importServiceFractory.Create();
    }
}
