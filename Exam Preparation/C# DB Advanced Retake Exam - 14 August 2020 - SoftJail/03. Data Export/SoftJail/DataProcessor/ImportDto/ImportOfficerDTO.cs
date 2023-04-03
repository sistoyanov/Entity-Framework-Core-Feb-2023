using SoftJail.Data.Models.Enums;
using SoftJail.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ImportDto;

[XmlType("Officer")]
public class ImportOfficerDTO
{
    [XmlElement("Name")]
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string FullName { get; set; } = null!;

    [XmlElement("Money")]
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    [XmlElement("Position")]
    [Required]
    public string Position { get; set; } = null!;

    [XmlElement("Weapon")]
    [Required]
    public string Weapon { get; set; } = null!;

    [XmlElement("DepartmentId")]
    [Required]
    public int DepartmentId { get; set; }

    [XmlArray("Prisoners")]
    public ImportOfficerPrisonerDTO[] Prisoners { get; set; } = null!;
}
