using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TmdbPersonMovie
{
    [JsonPropertyName("id")]
    public int MovieId { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    
    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; } = string.Empty;
}