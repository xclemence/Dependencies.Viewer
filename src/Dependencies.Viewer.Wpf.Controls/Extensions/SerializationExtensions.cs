using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Dependencies.Viewer.Wpf.Controls.Extensions
{
    public static class SerializationExtensions
    {
        public static Task<T> DeserializeObject<T>(this FileInfo xmlFile)
        {
            return Task.Run(() =>
            {
                using (var xmlReader = XmlReader.Create(xmlFile.FullName))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    var resultObject = (T)serializer.Deserialize(xmlReader);
                    return resultObject;
                }
            });
        }

        public static Task SerializeObject<T>(this T obj, string outFile)
        {
            return Task.Run(() =>
            {
                using (var writer = XmlWriter.Create(outFile))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, obj);
                }
            });
        }
    }
}
