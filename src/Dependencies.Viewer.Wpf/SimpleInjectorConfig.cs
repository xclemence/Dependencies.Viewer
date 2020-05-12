using System;
using System.Diagnostics.CodeAnalysis;
using Dependencies.Analyser.Base;
using Dependencies.Analyser.Native;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.Controls;
using Dependencies.Viewer.Wpf.Extensions;
using Dependencies.Viewer.Wpf.Layouts;
using Dragablz;
using MaterialDesignThemes.Wpf;
using SimpleInjector;

namespace Dependencies.Viewer.Wpf
{
    internal static class SimpleInjectorConfig
    {
        public static Container Container { get; private set; }

        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose method are call by IoC")]
        public static void Config()
        {
            Container = new Container();

            Container.Register<IInterTabClient, KeepOneInterLayoutClient>();
            Container.Register<ISettingProvider, SettingProvider>(Lifestyle.Singleton);
            Container.Register<INativeAnalyser, NativeAnalyser>(Lifestyle.Transient);
            Container.RegisterInstance<ISnackbarMessageQueue>(new SnackbarMessageQueue());


            Container.Register(typeof(IAnalyserServiceFactory<>), typeof(SimpleInjectorAnalyseServiceFactory<>), Lifestyle.Singleton);
            Container.Register(typeof(IExchangeServiceFactory<>), typeof(SimpleInjectorExchangeServiceFactory<>), Lifestyle.Singleton);
            Container.Register<AnalyserProvider>(Lifestyle.Singleton);

            Container.RegisterAnalyser();
            Container.RegisterExchange();

            Container.Options.SuppressLifestyleMismatchVerification = true;
            Container.Collection.Container.Options.SuppressLifestyleMismatchVerification = true;

            //Container.Verify();
        }

        private static void RegisterAnalyser(this Container container)
        {
            var pluginAssemblies = AppDomain.CurrentDomain.FindPluginAssemblies("Analyser", "Dependencies.Analyser*");

            container.Collection.Register<IAssemblyAnalyserFactory>(pluginAssemblies, Lifestyle.Singleton);
        }

        private static void RegisterExchange(this Container container)
        {
            var pluginAssemblies = AppDomain.CurrentDomain.FindPluginAssemblies("Exchange", "Dependencies.Exchange*");

            container.Collection.Register<IExportAssembly>(pluginAssemblies);
            container.Collection.Register<IImportAssembly>(pluginAssemblies);
            container.Collection.Register<ISettingUpdaterProvider>(pluginAssemblies);
            container.Register(typeof(ISettingServices<>), pluginAssemblies);
        }
    }
}
