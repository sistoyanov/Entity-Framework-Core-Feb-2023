using Artillery.Data.Models.Enums;
using Artillery.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Gun")]
    public class ExportGunDTO
    {
        [XmlAttribute("Manufacturer")]
        public string Manufacturer { get; set; }

        [XmlAttribute("GunType")]
        public string GunType { get; set; }

        [XmlAttribute("GunWeight")]
        public string GunWeight { get; set; }

        [XmlAttribute("BarrelLength")]
        public string BarrelLength { get; set; }

        [XmlAttribute("Range")]
        public string Range { get; set; }

        [XmlArray("Countries")]
        public ExportCountryDTO[] Countries { get; set; }

    }
}
