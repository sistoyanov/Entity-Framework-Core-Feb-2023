using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto;

[JsonObject]
public class ImportMailDTO
{
    [JsonProperty("Description")]
    [Required]
    public string Description { get; set; } = null!;

    [JsonProperty("Sender")]
    [Required]
    public string Sender { get; set; } = null!;

    [JsonProperty("Address")]
    [Required]
    [RegularExpression(@"^[A-Za-z0-9\s]+str\.$")]
    public string Address { get; set; } = null!;
}
