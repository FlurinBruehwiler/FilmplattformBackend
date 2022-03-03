using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models.TMDB;
using WebAPITest.Services.UserService;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class Search : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _apiKey;
    
    public Search(IConfiguration configuration, IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
    }
    
    [HttpGet("SearchMovies/{searchString}"), Authorize]
    public async Task<ActionResult<TMDBMovieSearchResult>> SearchMovies(string searchString)
    {
        searchString = Regex.Replace(searchString, @"\s+", "+");
        var url = $"search/movie?api_key={_apiKey}&query={searchString}";
        
        TMDBMovieSearchResult? tmdbSearcher;
        var client = _clientFactory.CreateClient("tmdb");
        
        try
        {
            tmdbSearcher = await client.GetFromJsonAsync<TMDBMovieSearchResult>(url);
            
            if (tmdbSearcher == null)
            {
                throw new Exception();
            }
        }
        catch (Exception)
        {
            return NotFound();
        }
        
        return tmdbSearcher;
    }
}