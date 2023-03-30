using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto;

[XmlType("User")]
public class ExportUserDTO
{
    [XmlAttribute("username")]
    public string UserName { get; set; } = null!;

    [XmlArray("Purchases")]
    public ExportPurchaseDTO[] Purchases { get; set; } = null!;

    [XmlElement("TotalSpent")]
    public decimal TotalSpent { get; set; }
}
