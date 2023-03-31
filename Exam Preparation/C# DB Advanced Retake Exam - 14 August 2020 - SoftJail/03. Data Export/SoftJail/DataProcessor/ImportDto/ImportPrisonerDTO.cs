using SoftJail.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SoftJail.DataProcessor.ImportDto;

[JsonObject]
public class ImportPrisonerDTO
{
    [JsonProperty("FullName")]
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string FullName { get; set; } = null!;

    [JsonProperty("Nickname")]
    [Required]
    [RegularExpression(@"^The [A-Z][a-z]*$")]
    public string Nickname { get; set; } = null!;

    [JsonProperty("Age")]
    [Required]
    [Range(18, 65)]
    public int Age { get; set; }

    [JsonProperty("IncarcerationDate")]
    [Required]
    public string IncarcerationDate { get; set; } = null!;

    [JsonProperty("ReleaseDate")]
    public string? ReleaseDate { get; set; }

    [JsonProperty("Bail")]
    [Range(0, double.MaxValue)]
    public decimal? Bail { get; set; }

    [JsonProperty("CellId")]
    public int? CellId { get; set; }

    [JsonProperty("Mails")]
    public ImportMailDTO[] Mails { get; set; } = null!;
}
