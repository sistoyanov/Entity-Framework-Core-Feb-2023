using Newtonsoft.Json;
using ProductShop.Models;
using System.Text.Json.Serialization;

namespace ProductShop.DTOs.Import;

[JsonObject]
public class ImportProductDTO
{
    [JsonProperty("Name")]
    public string Name { get; set; } = null!;

    [JsonProperty("Price")]
    public decimal Price { get; set; }

    [JsonProperty("SellerId")]
    public int SellerId { get; set; }

    [JsonProperty("BuyerId")]
    public int? BuyerId { get; set; }
}
