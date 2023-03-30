using System.Xml.Serialization;

namespace VaporStore.DataProcessor.ExportDto;

[XmlType("Purchase")]
public class ExportPurchaseDTO
{
    [XmlElement("Card")]
    public string Card { get; set; } = null!;

    [XmlElement("Cvc")]
    public string Cvc { get; set; } = null!;

    [XmlElement("Date")]
    public string Date { get; set; } = null!;

    [XmlElement("Game")]
    public ExportGameDTO Game { get; set; } = null!;
}
