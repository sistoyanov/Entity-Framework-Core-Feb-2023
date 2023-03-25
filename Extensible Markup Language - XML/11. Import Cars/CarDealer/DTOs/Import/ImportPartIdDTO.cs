using System.Xml.Serialization;

namespace CarDealer.DTOs.Import;

[XmlType("parts")]
public class ImportPartIdDTO
{
    [XmlElement("partId")]
    public int PartId { get; set; }
}
