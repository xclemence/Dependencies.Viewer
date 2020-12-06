using Dependencies.Analyser.Base;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Base;
using SimpleInjector;

namespace Dependencies.Viewer.Wpf.IoC
{
    public class SimpleInjectorServiceFactory<T> : IExchangeServiceFactory<T>, IAnalyserServiceFactory<T>, IServiceFactory<T>
        where T : class
    {
        private readonly Container container;

        public SimpleInjectorServiceFactory(Container container) => this.container = container;

        public T Create() => container.GetInstance<T>();
    }
}
