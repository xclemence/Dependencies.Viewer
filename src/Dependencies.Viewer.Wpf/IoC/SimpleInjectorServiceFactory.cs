using Dependencies.Analyser.Base;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Base;
using SimpleInjector;

namespace Dependencies.Viewer.Wpf.IoC
{
    public class SimpleInjectorServiceFactory : IExchangeServiceFactory, IAnalyserServiceFactory, IServiceFactory
    {
        private readonly Container container;

        public SimpleInjectorServiceFactory(Container container) => this.container = container;

        public T Create<T>() where T : class => container.GetInstance<T>();
    }
}
