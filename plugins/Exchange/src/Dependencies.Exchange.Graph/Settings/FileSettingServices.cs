using Dependencies.Exchange.Base;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Dependencies.Exchange.Graph.Settings
{
    public class FileSettingServices : ISettingServices<GraphSettings>
    {

        public FileSettingServices()
        {
            var assemblyLocation = typeof(FileSettingServices).Assembly.Location;

            Filename = $@"{ Path.GetDirectoryName(assemblyLocation) }\graph-setting.json";
        }

        public string Filename { get; set; }

        public GraphSettings GetSettings()
        {
            var serializeObject = File.ReadAllText(Filename);
            return JsonConvert.DeserializeObject<GraphSettings>(serializeObject);

        }

        public void SaveSettings(GraphSettings settings)
        {
            var serializeObject = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(Filename, serializeObject);
        }
    }
}
