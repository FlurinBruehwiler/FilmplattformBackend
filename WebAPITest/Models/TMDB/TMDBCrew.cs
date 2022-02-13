using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TMDBCrew : TMDBPerson
{
    [JsonPropertyName("department")]
    public string? Department { get; set; }
}