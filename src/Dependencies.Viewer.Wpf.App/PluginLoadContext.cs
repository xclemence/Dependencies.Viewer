using System.Reflection;
using System.Runtime.Loader;

namespace Dependencies.Viewer.Wpf.App
{
    internal class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;

        public PluginLoadContext(string pluginPath)
        {
            resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            var assemblyFromDefaultContext = Default.LoadFromAssemblyName(assemblyName);

            if (assemblyFromDefaultContext != null)
                return assemblyFromDefaultContext;

            string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);

            if (assemblyPath != null)
                return LoadFromAssemblyPath(assemblyPath);

            return null;
        }
    }
}
