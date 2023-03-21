using Newtonsoft.Json;
using ProductShop.Models;
using System.Text.Json.Serialization;

namespace ProductShop.DTOs.Import;

public class ImportCategoriesProductDTO
{
    [JsonProperty("CategoryId")]
    public int CategoryId { get; set; }

    [JsonProperty("ProductId")]
    public int ProductId { get; set; }
}
