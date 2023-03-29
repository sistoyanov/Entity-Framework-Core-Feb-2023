using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ExportDto;

[XmlType("Task")]
public class ExportTaskDTO
{
    [XmlElement("Name")]
    public string Name { get; set; } = null!;

    [XmlElement("Label")]
    public string Label { get; set; }
}
