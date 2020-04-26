using System;
using Dependencies.Exchange.Base;
using SimpleInjector;

namespace Dependencies.Viewer.Wpf.App
{
    public class SimpleInjectorExchangeServiceFactory<T> : IExchangeServiceFactory<T>
        where   T : class
    {
        public readonly Container container;

        public SimpleInjectorExchangeServiceFactory(Container container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public T Create() =>  container.GetInstance<T>();
    }
}
