using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Utilities;

internal class XmlHelper
{
    public static T Deserialize<T>(string inputXml, string rootName)
    {
        XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

        using (StringReader reader = new StringReader(inputXml))
        {
            T deserializedDtos = (T)xmlSerializer.Deserialize(reader)!;

            return deserializedDtos;
        }

    }

    public static string Serialize<T>(T inputObj, string rootName)
    {
        StringBuilder output = new StringBuilder();

        XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);

        XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRoot);

        XmlSerializerNamespaces nameSpaces = new XmlSerializerNamespaces();
        nameSpaces.Add(string.Empty, string.Empty);

        using (StringWriter writer = new StringWriter(output))
        {
            serializer.Serialize(writer, inputObj, nameSpaces);

            return output.ToString().TrimEnd();
        }
    }
}
