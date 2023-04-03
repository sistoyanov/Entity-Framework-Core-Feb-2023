using System.Xml.Linq;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto;

[XmlType("Prisoner")]
public class ExportPrisonerDTO
{
    [XmlElement("Id")]
    public int Id { get; set; }

    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("IncarcerationDate")]
    public string IncarcerationDate { get; set; } = null!;

    [XmlArray("EncryptedMessages")]
    public ExportMessageDTO[] EncryptedMessages { get; set; } = null!;

}
