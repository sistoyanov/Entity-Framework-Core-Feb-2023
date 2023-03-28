using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ImportDto;

[XmlType("Cast")]
public class ImportCastDTO
{
    [XmlElement("FullName")]
    [Required]
    [StringLength(30, MinimumLength = 4)]
    public string FullName { get; set; } = null!;

    [XmlElement("IsMainCharacter")]
    [Required]
    public bool IsMainCharacter { get; set; }

    [XmlElement("PhoneNumber")]
    [Required]
    [RegularExpression(@"^\+44-\d{2}-\d{3}-\d{4}$")]
    public string PhoneNumber { get; set; } = null!;

    [XmlElement("PlayId")]
    [Required]
    public int PlayId { get; set; }
}
