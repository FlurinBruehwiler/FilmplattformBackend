using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

public class TMDBPerson
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("original_name")]
    public string? Name { get; set; }
}