using System.Reflection;
using System.Runtime.Loader;

namespace Dependencies.Viewer.Wpf
{
    internal class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver resolver;
        private readonly bool tryDefaultContextLoading;

        public PluginLoadContext(string pluginPath, bool tryDefaultContextLoading = false)
        {
            resolver = new AssemblyDependencyResolver(pluginPath);
            this.tryDefaultContextLoading = tryDefaultContextLoading;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Assembly assembly = null;
            if (tryDefaultContextLoading)
                assembly = LoadOnDefaultContext(assemblyName);

            return assembly ?? LoadOnPluginContext(assemblyName);
        }

        private static Assembly LoadOnDefaultContext(AssemblyName assemblyName)
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
            return null;
        }

        private Assembly LoadOnPluginContext(AssemblyName assemblyName)
        {
            var assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);

            if (assemblyPath != null)
                return LoadFromAssemblyPath(assemblyPath);

            return null;
        }

    }
}
