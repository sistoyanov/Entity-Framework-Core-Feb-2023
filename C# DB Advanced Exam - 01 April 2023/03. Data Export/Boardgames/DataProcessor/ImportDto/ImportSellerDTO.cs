using Boardgames.Data.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Boardgames.DataProcessor.ImportDto;

[JsonObject]
public class ImportSellerDTO
{
    [JsonProperty("Name")]
    [Required]
    [StringLength(20, MinimumLength = 5)]
    public string Name { get; set; } = null!;

    [JsonProperty("Address")]
    [Required]
    [StringLength(30, MinimumLength = 2)]
    public string Address { get; set; } = null!;

    [JsonProperty("Country")]
    [Required]
    public string Country { get; set; } = null!;

    [JsonProperty("Website")]
    [Required]
    [RegularExpression(@"^www\.[a-zA-Z0-9\-]+\.com$")]
    public string Website { get; set; } = null!;

    [JsonProperty("Boardgames")]
    public int[] Boardgames { get; set; } = null!;
}
