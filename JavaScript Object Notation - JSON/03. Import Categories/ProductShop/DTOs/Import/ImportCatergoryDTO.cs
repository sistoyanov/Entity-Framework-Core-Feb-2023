using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ProductShop.DTOs.Import;

[JsonObject]
public class ImportCatergoryDTO
{
    [JsonProperty("name")]
    public string? Name { get; set; }
}
