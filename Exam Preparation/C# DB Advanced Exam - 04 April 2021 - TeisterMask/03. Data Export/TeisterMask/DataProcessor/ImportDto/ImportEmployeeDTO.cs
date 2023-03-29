using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TeisterMask.Data.Models;

namespace TeisterMask.DataProcessor.ImportDto;

[JsonObject]
public class ImportEmployeeDTO
{
    [JsonProperty("Username")]
    [Required]
    [StringLength(40, MinimumLength = 3)]
    [RegularExpression(@"^[a-zA-Z0-9]*$")]
    public string Username { get; set; } = null!;

    [JsonProperty("Email")]
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [JsonProperty("Phone")]
    [Required]
    [RegularExpression(@"^\d{3}-\d{3}-\d{4}$")]
    public string Phone { get; set; } = null!;

    [JsonProperty("Tasks")]
    public int[] Tasks { get; set; } = null!;
}
