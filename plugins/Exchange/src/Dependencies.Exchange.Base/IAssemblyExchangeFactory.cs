namespace Dependencies.Exchange.Base
{
    public interface IAssemblyExchangeFactory
    {
        string Name { get; }

        string Code { get; }

        IExportAssembly GetExportService();

        IImportAssembly GetImportService();
    }
}
