using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace VaporStore.DataProcessor.ImportDto;

[JsonObject]
public class ImportGameDTO
{
    [JsonProperty("Name")]
    [Required]
    public string Name { get; set; } = null!;

    [JsonProperty("Price")]
    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [JsonProperty("ReleaseDate")]
    [Required]
    public string ReleaseDate { get; set; }

    [JsonProperty("Developer")]
    [Required]
    public string Developer { get; set; } = null!;

    [JsonProperty("Genre")]
    [Required]
    public string Genre { get; set; } = null!;

    [JsonProperty("Tags")]
    public string[] Tags { get; set; } = null!;
}
