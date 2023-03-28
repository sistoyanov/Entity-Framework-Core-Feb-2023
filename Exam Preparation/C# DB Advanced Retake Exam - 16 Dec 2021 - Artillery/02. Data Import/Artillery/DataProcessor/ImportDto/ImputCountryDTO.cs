﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto;

[XmlType("Country")]
public class ImputCountryDTO
{
    [XmlElement("CountryName")]
    [Required]
    [StringLength(60, MinimumLength = 4)]
    public string CountryName { get; set; } = null!;

    [XmlElement("ArmySize")]
    [Required]
    [Range(50_000, 10_000_000)]
    public int ArmySize { get; set; }
}
