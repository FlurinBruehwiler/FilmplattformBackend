using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TmdbMovieCrew : TMDBPerson
{
    [JsonPropertyName("department")]
    public string? Department { get; set; }
}