using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Trucks.Data.Models.Enums;

namespace Trucks.DataProcessor.ImportDto;

[XmlType("Truck")]
public class ImportTruckDTO
{
    [XmlElement("RegistrationNumber")]
    [StringLength(8)]
    [RegularExpression("^[A-Z]{2}\\d{4}[A-Z]{2}$")]
    public string RegistrationNumber { get; set; } = null!;

    [XmlElement("VinNumber")]
    [StringLength(17)]
    public string VinNumber { get; set; } = null!;

    [XmlElement("TankCapacity")]
    [Range(950, 1420)]
    public int TankCapacity { get; set; }

    [XmlElement("CargoCapacity")]
    [Range(5000, 29000)]
    public int CargoCapacity { get; set; }

    [XmlElement("CategoryType")]
    public CategoryType CategoryType { get; set; }

    [XmlElement("MakeType")]
    public MakeType MakeType { get; set; }
}
