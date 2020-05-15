using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dependencies.Viewer.Wpf.IoC;

namespace Dependencies.Viewer.Wpf.Extensions
{
    internal static class AssemblyFileLoaderExtensions
    {
        internal static Assembly LoadPluginAssembly(this FileInfo file)
        {
            var loadContext = new PluginLoadContext(file.FullName);
            var assembly = loadContext.LoadFromAssemblyPath(file.FullName);
            return assembly;
        }

        internal static IList<Assembly> FindPluginAssemblies(this AppDomain appDomain, string pluginName, string pluginAssemblyPattern)
        {
            var pluginDirectory = Path.Combine(appDomain.BaseDirectory, "Plugins", pluginName);

            if (!Directory.Exists(pluginDirectory))
                return new List<Assembly>();

            var files = (new DirectoryInfo(pluginDirectory)).GetFiles(pluginAssemblyPattern, SearchOption.AllDirectories);

            return files.Where(x => x.Extension == ".dll").Select(LoadPluginAssembly).ToList();
        }
    }
}
