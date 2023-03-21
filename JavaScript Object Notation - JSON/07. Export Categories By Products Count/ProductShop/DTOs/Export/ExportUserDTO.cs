using Newtonsoft.Json;
using ProductShop.Models;

namespace ProductShop.DTOs.Export;

[JsonObject]
public class ExportUserDTO
{
    [JsonProperty("firstName")]
    public string? FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; } = null!;

    [JsonProperty("soldProducts")]
    public ICollection<ExportSoldProductDTO>? SoldProducts { get; set; }
}
