using System;
using Dependencies.Analyser.Base;
using SimpleInjector;

namespace Dependencies.Viewer.Wpf.App
{
    public class SimpleInjectorServiceFactory<T> : IServiceFactory<T>
        where   T : class
    {
        public readonly Container container;

        public SimpleInjectorServiceFactory(Container container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public T Create() =>  container.GetInstance<T>();
    }
}
