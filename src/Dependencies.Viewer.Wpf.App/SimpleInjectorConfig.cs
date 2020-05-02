using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows.Navigation;
using Dependencies.Analyser.Base;
using Dependencies.Analyser.Native;
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
            
            Container.Register(typeof(IAnalyserServiceFactory<>), typeof(SimpleInjectorServiceFactory<>));
            Container.Register<AnalyserProvider>(Lifestyle.Singleton);
            RegisterAnalyser(Container);

            Container.Options.SuppressLifestyleMismatchVerification = true;
            Container.Collection.Container.Options.SuppressLifestyleMismatchVerification = true;

        }

        private static void RegisterAnalyser(Container container)
        {
            string pluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "Analyser");

            if (!Directory.Exists(pluginDirectory)) return;

            var files = new DirectoryInfo(pluginDirectory).GetFiles("Dependencies.Analyser*", SearchOption.AllDirectories);

            
            var pluginAssemblies = files.Where(x => x.Extension == ".dll" && x.Name != "Dependencies.Analyser.Base.dll").Select(x =>
            {
                PluginLoadContext loadContext = new PluginLoadContext(x.FullName);
                var assembly =  loadContext.LoadFromAssemblyPath(x.FullName);
                return assembly;
            }).ToList();

            container.Collection.Register<IAssemblyAnalyserFactory>(pluginAssemblies);
            container.Collection.Register<IAssemblyAnalyser>(pluginAssemblies);
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
