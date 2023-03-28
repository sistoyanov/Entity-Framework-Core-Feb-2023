using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Theatre.Data.Models;

namespace Theatre.DataProcessor.ImportDto;

[JsonObject]
public class ImportTheatreDTO
{
    [JsonProperty("Name")]
    [Required]
    [StringLength(30, MinimumLength = 4)]
    public string Name { get; set; } = null!;

    [JsonProperty("NumberOfHalls")]
    [Required]
    [Range(1, 10)]
    public sbyte NumberOfHalls { get; set; }

    [JsonProperty("Director")]
    [Required]
    [StringLength(30, MinimumLength = 4)]
    public string Director { get; set; } = null!;

    [JsonProperty("Tickets")]
    public ImportTicketDTO[] Tickets { get; set; } = null!;
}
