using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TMDBGenre
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}