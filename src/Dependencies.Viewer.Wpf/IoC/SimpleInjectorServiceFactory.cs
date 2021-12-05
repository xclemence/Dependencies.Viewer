using Dependencies.Analyser.Base;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls.Base;
using SimpleInjector;

namespace Dependencies.Viewer.Wpf.IoC;

public class SimpleInjectorServiceFactory : IExchangeServiceFactory, IAnalyserServiceFactory, IServiceFactory
{
    private readonly Scope scope;

    public SimpleInjectorServiceFactory(Scope scope) => this.scope = scope;

    public T Create<T>() where T : class => scope.GetInstance<T>();
}
