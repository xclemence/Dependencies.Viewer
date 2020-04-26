using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Dependencies.Analyser.Base;
using Dependencies.Analyser.Native;
using Dependencies.Exchange.Base;
using Dependencies.Viewer.Wpf.App.Layouts;
using Dependencies.Viewer.Wpf.Controls;
using Dragablz;
using MaterialDesignThemes.Wpf;
using SimpleInjector;

namespace Dependencies.Viewer.Wpf.App
{
    internal static class SimpleInjectorConfig
    {
        public static Container Container { get; private set; }

        public static void Config()
        {
            Container = new Container();

            Container.Register<IInterTabClient, KeepOneInterLayoutClient>();
            Container.Register<ISettingProvider, SettingProvider>(Lifestyle.Singleton);
            Container.Register<INativeAnalyser, NativeAnalyser>(Lifestyle.Transient);
            Container.RegisterInstance<ISnackbarMessageQueue>(new SnackbarMessageQueue());
            
         
            Container.Register(typeof(IAnalyserServiceFactory<>), typeof(SimpleInjectorAnalyseServiceFactory<>));
            Container.Register(typeof(IExchangeServiceFactory<>), typeof(SimpleInjectorExchangeServiceFactory<>));
            Container.Register<AnalyserProvider>(Lifestyle.Singleton);

            Container.RegisterAnalyser();
            Container.RegisterExchange();


            Container.Options.SuppressLifestyleMismatchVerification = true;
            Container.Collection.Container.Options.SuppressLifestyleMismatchVerification = true;

        }

        private static void RegisterAnalyser(this Container container)
        {
            string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "Analyser");

            if (!Directory.Exists(pluginDirectory))
                Directory.CreateDirectory(pluginDirectory);

            var files = new DirectoryInfo(pluginDirectory).GetFiles("Dependencies.Analyser*", SearchOption.AllDirectories);
            
            var pluginAssemblies = files.Where(x => x.Extension == ".dll").Select(x =>
            {
                PluginLoadContext loadContext = new PluginLoadContext(x.FullName);
                var assembly =  loadContext.LoadFromAssemblyPath(x.FullName);
                return assembly;
            }).ToList();

            container.Collection.Register<IAssemblyAnalyserFactory>(pluginAssemblies);
            container.Collection.Register<IAssemblyAnalyser>(pluginAssemblies);
        }

        private static void RegisterExchange(this Container container)
        {
            string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "Exchange");

            if (!Directory.Exists(pluginDirectory))
                Directory.CreateDirectory(pluginDirectory);

            var files = new DirectoryInfo(pluginDirectory).GetFiles("Dependencies.Exchange*", SearchOption.AllDirectories);

            var pluginAssemblies = files.Where(x => x.Extension == ".dll").Select(x =>
            {
                PluginLoadContext loadContext = new PluginLoadContext(x.FullName);
                var assembly = loadContext.LoadFromAssemblyPath(x.FullName);
                return assembly;
            }).ToList();

            container.Collection.Register<IAssemblyExchangeFactory>(pluginAssemblies);
            container.Collection.Register<IExportAssembly>(pluginAssemblies);
            container.Collection.Register<IImportAssembly>(pluginAssemblies);
        }
    }

    class PluginLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver resolver;

        public PluginLoadContext(string pluginPath)
        {
            resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);

            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }
}
