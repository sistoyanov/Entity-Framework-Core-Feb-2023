using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatcherDTO
    {
        [XmlElement("Name")]
        [Required]
        [MaxLength(40)]
        [MinLength(2)]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string? Position { get; set; }

        [XmlArray("Trucks")]
        public ImportTruckDTO[] Trucks { get; set; } = null!;
    }
}
