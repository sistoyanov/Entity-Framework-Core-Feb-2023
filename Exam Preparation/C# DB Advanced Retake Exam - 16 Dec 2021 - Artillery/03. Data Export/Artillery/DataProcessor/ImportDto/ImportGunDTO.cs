using Artillery.Data.Models.Enums;
using Artillery.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;

namespace Artillery.DataProcessor.ImportDto;

[JsonObject]
public class ImportGunDTO
{
    [JsonProperty("ManufacturerId")]
    [Required]
    public int ManufacturerId { get; set; }

    [JsonProperty("GunWeight")]
    [Required]
    [Range(100, 1_350_000)]
    public int GunWeight { get; set; }

    [JsonProperty("BarrelLength")]
    [Required]
    [Range(2.0, 35.0)]
    public double BarrelLength { get; set; }

    [JsonProperty("NumberBuild")]
    public int? NumberBuild { get; set; }

    [JsonProperty("Range")]
    [Required]
    [Range(1, 100_000)]
    public int Range { get; set; }

    [JsonProperty("GunType")]
    [Required]
    public string? GunType { get; set; } 

    [JsonProperty("ShellId")]
    [Required]
    public int ShellId { get; set; }

    [JsonProperty("Countries")]
    public ImportCountryIdDTO[] Countries { get; set; } = null!;
}
