using WebAPITest.Models.TMDB;

namespace WebAPITest.Services;

public class TmdbService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _apiKey;
    
    public TmdbService(IConfiguration configuration, IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
    }
    
    public async Task<TMDBMovieDetails?> GetMovieDetails(int id)
    {
        var url = $"movie/{id}?api_key={_apiKey}";

        TMDBMovieDetails? tmdbMovie;
        var client = _clientFactory.CreateClient("tmdb");

        try
        {
            tmdbMovie = await client.GetFromJsonAsync<TMDBMovieDetails>(url);
        }
        catch (Exception)
        {
            return new TMDBMovieDetails();
        }

        return tmdbMovie;
    }
}