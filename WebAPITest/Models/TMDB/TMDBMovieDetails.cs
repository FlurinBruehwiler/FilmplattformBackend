using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TMDBMovieDetails
{
    [JsonPropertyName("genres")]
    public List<TMDBGenre>? Genres { get; set; }
    
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("original_title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("overview")]
    public string? LongDescription { get; set; }
    
    [JsonPropertyName("release_date")]
    public string? ReleaseDate { get; set; }
    
    [JsonPropertyName("tagline")]
    public string? ShortDescription { get; set; }
}