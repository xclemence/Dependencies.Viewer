using System;
using Dependencies.Analyser.Base;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.ApplicationSettings;
using Dependencies.Viewer.Wpf.Controls;
using Dependencies.Viewer.Wpf.Controls.Base;
using Dependencies.Viewer.Wpf.Controls.Services;
using Dependencies.Viewer.Wpf.Extensions;
using Dependencies.Viewer.Wpf.Layouts;
using Dragablz;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using SimpleInjector;

namespace Dependencies.Viewer.Wpf.IoC
{
    internal static class SimpleInjectorConfig
    {
        public static Container Container { get; private set; } = default!; // Mandatory for application

        public static void Config(IConfigurationRoot configurationRoot)
        {
            Container = new Container();

            Container.Register<IInterTabClient, KeepOneInterLayoutClient>();
            Container.Register<IAnalyserSettingProvider, AnalyserSettingProvider>(Lifestyle.Singleton);
            Container.Register<IApplicationSettingProvider, ApplicationSettingProvider>(Lifestyle.Singleton);
            Container.RegisterInstance<ISnackbarMessageQueue>(new SnackbarMessageQueue());

            Container.RegisterInstance(LoggerFactory.Create(x => x.AddNLog(configurationRoot)));
            Container.Register(typeof(ILogger<>), typeof(Logger<>), Lifestyle.Transient);

            Container.Register(typeof(IAnalyserServiceFactory<>), typeof(SimpleInjectorServiceFactory<>), Lifestyle.Singleton);
            Container.Register(typeof(AppLoggerService<>), typeof(AppLoggerService<>), Lifestyle.Singleton);

            Container.Register(typeof(IExchangeServiceFactory<>), typeof(SimpleInjectorServiceFactory<>), Lifestyle.Singleton);
            Container.Register(typeof(IServiceFactory<>), typeof(SimpleInjectorServiceFactory<>), Lifestyle.Singleton);
            Container.Register<AnalyserProvider>(Lifestyle.Singleton);
            Container.Register<ThemeManager>(Lifestyle.Singleton);

            Container.RegisterAnalyser();
            Container.RegisterExchange();

            Container.Options.ResolveUnregisteredConcreteTypes = true;
            Container.Options.SuppressLifestyleMismatchVerification = true;
            Container.Collection.Container.Options.SuppressLifestyleMismatchVerification = true;
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
