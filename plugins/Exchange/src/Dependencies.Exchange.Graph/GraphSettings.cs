using Dependencies.Exchange.Base;

namespace Dependencies.Exchange.Graph
{
    public class GraphSettings : IExchangeSettings
    {
        public string ServiceUri { get; set; } = "http://localhost:5001";
    }
}
