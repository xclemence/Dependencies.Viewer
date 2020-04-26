namespace Dependencies.Exchange.Base
{
    public interface IExchangeServiceFactory<T>
    {
        T Create();
    }
}
