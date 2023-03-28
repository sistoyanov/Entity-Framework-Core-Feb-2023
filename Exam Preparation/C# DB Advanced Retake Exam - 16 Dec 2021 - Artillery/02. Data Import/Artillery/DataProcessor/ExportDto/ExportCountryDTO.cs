using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto;

[XmlType("Country")]
public class ExportCountryDTO
{
    [XmlAttribute("Country")]
    public string Country { get; set; } = null!;

    [XmlAttribute("ArmySize")]
    public string ArmySize { get; set; }
}
