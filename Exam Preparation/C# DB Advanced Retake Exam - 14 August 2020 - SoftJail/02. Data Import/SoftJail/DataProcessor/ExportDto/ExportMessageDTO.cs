using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto;

[XmlType("Message")]
public class ExportMessageDTO
{
    [XmlElement("Description")]
    public string Description { get; set; } = null!;
}
