using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TMDBMovieDetails
{
    [JsonPropertyName("genres")]
    public List<TMDBGenre>? Genres { get; set; }
    
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("original_title")]
    public string Title { get; set; } = string.Empty;
    
    [JsonPropertyName("overview")]
    public string LongDescription { get; set; } = string.Empty;
    
    [JsonPropertyName("release_date")]
    public string ReleaseDate { get; set; } = string.Empty;
    
    [JsonPropertyName("tagline")]
    public string ShortDescription { get; set; } = string.Empty;

    [JsonPropertyName("backdrop_path")]
    public string BackdropPath { get; set; } = string.Empty;
    
    [JsonPropertyName("poster_path")]
    public string PosterPath { get; set; } = string.Empty;
}