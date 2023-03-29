using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto;

[XmlType("Task")]
public class ImportTaskDTO
{

    [XmlElement("Name")]
    [Required]
    [StringLength(40, MinimumLength = 2)]
    public string Name { get; set; } = null!;

    [XmlElement("OpenDate")]
    [Required]
    public string OpenDate { get; set; } = null!;

    [XmlElement("DueDate")]
    [Required]
    public string DueDate { get; set; } = null!;

    [XmlElement("ExecutionType")]
    [Required]
    public ExecutionType ExecutionType { get; set; }

    [XmlElement("LabelType")]
    [Required]
    public LabelType LabelType { get; set; }
}
