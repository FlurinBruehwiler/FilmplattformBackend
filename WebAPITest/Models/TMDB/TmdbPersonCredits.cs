using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TmdbPersonCredits
{
    [JsonPropertyName("original_title")]
    public int Id { get; set; }
    
    [JsonPropertyName("cast")]
    public List<TmdbPersonMovie> Cast { get; set; } = new();
    
    [JsonPropertyName("crew")]
    public List<TmdbPersonMovie> Crew { get; set; } = new();
}