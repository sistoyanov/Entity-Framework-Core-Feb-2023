using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CarDealer.DTOs.Import;

[JsonObject]
public class ImportPartDTO
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("quantity")]
    public int Quantity { get; set; }

    [JsonProperty("supplierId")]
    public int SupplierId { get; set; }
}
