using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models;

namespace VaporStore.DataProcessor.ImportDto;

[JsonObject]
public class ImportUserDTO
{
    [JsonProperty("FullName")]
    [Required]
    [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
    public string FullName { get; set; } = null!;

    [JsonProperty("Username")]
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string Username { get; set; } = null!;

    [JsonProperty("Email")]
    [Required]
    public string Email { get; set; } = null!;

    [JsonProperty("Age")]
    [Required]
    [Range(3, 103)]
    public int Age { get; set; }

    [JsonProperty("Cards")]
    public ImportCardDTO[] Cards { get; set; }
}
