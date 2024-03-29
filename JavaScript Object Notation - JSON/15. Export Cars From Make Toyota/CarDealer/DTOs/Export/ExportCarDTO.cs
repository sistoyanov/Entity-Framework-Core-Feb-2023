﻿using Newtonsoft.Json;

namespace CarDealer.DTOs.Export;

[JsonObject]
public class ExportCarDTO
{
    [JsonProperty("Id")]
    public int Id { get; set; }

    [JsonProperty("Make")]
    public string Make { get; set; } = null!;

    [JsonProperty("Model")]
    public string Model { get; set; } = null!;

    [JsonProperty("TraveledDistance")]
    public long TraveledDistance { get; set; }
}
