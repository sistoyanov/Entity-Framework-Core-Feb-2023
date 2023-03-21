using Newtonsoft.Json;

namespace ProductShop.DTOs.Export;

[JsonObject]
public class ExportSoldProductDTO
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("buyerFirstName")]
    public string BuyerFirstName { get; set; } = null!;

    [JsonProperty("buyerLastName")]
    public string BuyerLastName { get; set; } = null!;
}
