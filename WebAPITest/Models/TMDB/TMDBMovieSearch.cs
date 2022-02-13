using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TMDBMovieSearch
{
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }
    
    [JsonPropertyName("original_title")]
    public string? Title { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }
}