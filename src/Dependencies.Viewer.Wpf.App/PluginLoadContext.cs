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
            try
            {
                var assemblyFromDefaultContext = Default.LoadFromAssemblyName(assemblyName);

                if (assemblyFromDefaultContext != null)
                    return assemblyFromDefaultContext;
            }
            catch
            {
                // Try load with path
            }

            string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);

            if (assemblyPath != null)
                return LoadFromAssemblyPath(assemblyPath);

            return null;
        }
    }
}
