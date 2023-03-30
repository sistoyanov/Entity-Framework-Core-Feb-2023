using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enums;

namespace VaporStore.DataProcessor.ImportDto;

[JsonObject]
public class ImportCardDTO
{
    [JsonProperty("Number")]
    [Required]
    [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$")]
    public string Number { get; set; } = null!;

    [JsonProperty("CVC")]
    [Required]
    [RegularExpression(@"^\d{3}$")]
    public string Cvc { get; set; } = null!;

    [JsonProperty("Type")]
    [Required]
    public CardType Type { get; set; }
}
