using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Trucks.Data.Models;

namespace Trucks.DataProcessor.ImportDto;

[JsonObject]
public class ImportClientDTO
{
    [JsonProperty("Name")]
    [Required]
    [MaxLength(40)]
    [MinLength(3)]
    public string Name { get; set; } = null!;

    [JsonProperty("Nationality")]
    [Required]
    [MaxLength(40)]
    [MinLength(2)]
    public string Nationality { get; set; } = null!;

    [JsonProperty("Type")]
    [Required]
    public string Type { get; set; } = null!;

    [JsonProperty("Trucks")]
    public virtual int[] Trucks { get; set; } = null!;
}
