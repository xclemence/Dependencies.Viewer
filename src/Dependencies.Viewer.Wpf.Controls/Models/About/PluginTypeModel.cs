
using System.Collections.Generic;

namespace Dependencies.Viewer.Wpf.Controls.Models.About
{
    public record PluginTypeModel
    {
        public PluginTypeModel(string type, IReadOnlyList<PluginModel> names)
        {
            Type = type;
            Names = names;
        }

        public string Type { get; }

        public IReadOnlyList<PluginModel> Names { get; }
    }
}
