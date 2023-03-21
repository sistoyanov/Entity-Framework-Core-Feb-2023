using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ProductShop.DTOs.Export;

[JsonObject]
public class ExportCategoriesByProductsCountDTO
{
    [JsonProperty("category")]
    public string Name { get; set; } = null!;

    [JsonProperty("productsCount")]
    public int ProductsCount { get; set; }

    [JsonProperty("averagePrice")]
    public string AveragePrice { get; set; }

    [JsonProperty("totalRevenue")]
    public string TotalRevenue { get; set; }
}
