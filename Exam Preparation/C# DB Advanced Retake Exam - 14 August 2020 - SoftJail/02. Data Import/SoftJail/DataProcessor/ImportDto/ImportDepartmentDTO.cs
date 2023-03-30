using Newtonsoft.Json;
using SoftJail.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto;

[JsonObject]
public class ImportDepartmentDTO
{
    [JsonProperty("Name")]
    [Required]
    [StringLength(25, MinimumLength = 3)]
    public string Name { get; set; } = null!;

    [JsonProperty("Cells")]
    public ImportCellDTO[] Cells { get; set; } = null!;
}
