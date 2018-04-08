namespace KPlugin
{
    using System.Xml.Serialization;
    using System.IO;

    public static class Serializer
    {
        public static string Serialize<T>(T content)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, content);

                return writer.ToString();
            }
        }

        public static T Deserialize<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
