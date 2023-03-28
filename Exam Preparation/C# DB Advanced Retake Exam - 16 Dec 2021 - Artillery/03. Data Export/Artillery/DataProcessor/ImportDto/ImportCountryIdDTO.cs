using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Artillery.DataProcessor.ImportDto;

[JsonObject]
public class ImportCountryIdDTO
{
    [JsonProperty("Id")]
    public int Id { get; set; }
}
