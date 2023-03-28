using Footballers.Data.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Footballers.DataProcessor.ImportDto;

[JsonObject]
public class ImportTeamDTO
{
    [JsonProperty("Name")]
    [Required]
    [MinLength(3)]
    [MaxLength(40)]
    [RegularExpression("^[A-Za-z0-9 .-]+$")]
    public string Name { get; set; } = null!;

    [JsonProperty("Nationality")]
    [Required]
    [MinLength(2)]
    [MaxLength(40)]
    public string Nationality { get; set; } = null!;

    [JsonProperty("Trophies")]
    [Required]
    public int Trophies { get; set; }

    [JsonProperty("Footballers")]
    public virtual int[] Footballers { get; set; } = null!;
}
