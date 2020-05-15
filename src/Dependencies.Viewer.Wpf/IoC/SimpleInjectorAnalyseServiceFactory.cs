using System;
using Dependencies.Analyser.Base;
using SimpleInjector;

namespace Dependencies.Viewer.Wpf.IoC
{
    public class SimpleInjectorAnalyseServiceFactory<T> : IAnalyserServiceFactory<T>
        where T : class
    {
        private readonly Container container;

        public SimpleInjectorAnalyseServiceFactory(Container container) => this.container = container ?? throw new ArgumentNullException(nameof(container));

        public T Create() => container.GetInstance<T>();
    }
}
