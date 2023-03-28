using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Theatre.Data.Models.Enums;

namespace Theatre.DataProcessor.ImportDto;

[XmlType("Play")]
public class ImportPlayDTO
{
    [XmlElement("Title")]
    [Required]
    [StringLength(50, MinimumLength = 4)]
    public string Title { get; set; } = null!;

    [XmlElement("Duration")]
    [Required]
    public string Duration { get; set; } = null!;

    [XmlElement("Raiting")]
    [Required]
    [Range(0.00, 10.00)]
    public float Rating { get; set; }

    [XmlElement("Genre")]
    [Required]
    public string Genre { get; set; } = null!;

    [XmlElement("Description")]
    [Required]
    [StringLength(700)]
    public string Description { get; set; } = null!;

    [XmlElement("Screenwriter")]
    [Required]
    [StringLength(30, MinimumLength = 4)]
    public string Screenwriter { get; set; } = null!;
}
