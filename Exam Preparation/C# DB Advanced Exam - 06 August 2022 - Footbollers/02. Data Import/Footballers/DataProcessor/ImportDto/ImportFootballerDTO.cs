using Footballers.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto;

[XmlType("Footballer")]
public class ImportFootballerDTO
{
    [XmlElement("Name")]
    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    public string Name { get; set; } = null!;

    [XmlElement("ContractStartDate")]
    [Required]
    public string ContractStartDate { get; set; }

    [XmlElement("ContractEndDate")]
    [Required]
    public string ContractEndDate { get; set; }

    [XmlElement("PositionType")]
    [Required]
    public PositionType PositionType { get; set; }

    [XmlElement("BestSkillType")]
    [Required]
    public BestSkillType BestSkillType { get; set; }
}
