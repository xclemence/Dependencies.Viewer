
namespace Dependencies.Viewer.Wpf.Controls.Models.About
{
    public record PluginModel
    {
        public PluginModel(string name, string version)
        {
            Name = name;
            Version = version;
        }

        public string Name { get; }
        public string Version { get; }
    }
}
