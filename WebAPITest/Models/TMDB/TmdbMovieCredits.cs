using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TmdbMovieCredits
{
    [JsonPropertyName("cast")]
    public List<TmdbMovieCast>? Cast { get; set; }
    
    [JsonPropertyName("crew")]
    public List<TmdbMovieCrew>? Crew { get; set; }
}

