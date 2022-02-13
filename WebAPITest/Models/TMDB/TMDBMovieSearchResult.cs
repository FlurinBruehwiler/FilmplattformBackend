using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WebAPITest.Models.TMDB;

public class TMDBMovieSearchResult
{
    [JsonPropertyName("page")]
    public int Page { get; set; }
    
    [JsonPropertyName("results")]
    public List<TMDBMovieSearch>? Results { get; set; }
    
    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }
    
    [JsonPropertyName("total_results")]
    public int TotalResults { get; set; }
}