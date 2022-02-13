using System.Text.Json.Serialization;

namespace WebAPITest.Models.TMDB;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class TMDBCredits
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("cast")]
    public List<TMDBCast>? Cast { get; set; }
    
    [JsonPropertyName("crew")]
    public List<TMDBCrew>? Crew { get; set; }
}

