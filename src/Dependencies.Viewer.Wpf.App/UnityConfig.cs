using CommonServiceLocator;
using Dependencies.Analyser.Base;
using Dependencies.Analyser.Mono;
using Dependencies.Viewer.Wpf.App.Layouts;
using Dragablz;
using Unity;
using Unity.ServiceLocation;

namespace Dependencies.Viewer.Wpf.App
{
    public static class UnityConfig
    {
        public static void Config()
        {
            var container = new UnityContainer();

            container.RegisterType<IInterTabClient, KeepOneInterLayoutClient>();
            container.RegisterType<IAssemblyAnalyser, MonoAnalyser>();

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }
    }
}
