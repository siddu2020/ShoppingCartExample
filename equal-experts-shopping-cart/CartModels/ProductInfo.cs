using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CartModels;

public class ProductInfo
{
    [Required, JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}
