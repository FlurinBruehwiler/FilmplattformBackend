using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Models.TMDB;
using WebAPITest.Services.UserService;
using WebAPITest.TmdbImports;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly FilmplattformContext _db;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IUserService _userService;
    private readonly string _apiKey;
    private readonly MovieImporter _movieImporter;

    public MoviesController(FilmplattformContext db, IHttpClientFactory clientFactory, IConfiguration configuration, IUserService userService)
    {
        _db = db;
        _clientFactory = clientFactory;
        _userService = userService;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
        _movieImporter = new MovieImporter(clientFactory, configuration, db);
    }
    
    [HttpGet("SearchMovies/{searchString}")]
    public async Task<ActionResult<TMDBMovieSearchResult>> SearchMovies(string searchString)
    {
        var userName = User?.Identity?.Name;
        
        Console.WriteLine(userName);
        
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

    [HttpGet("{id}")]
    public async Task<ActionResult<DtoMovie>> GetMovie(int id)
    {
        if (!MovieExists(id))
        {
            await _movieImporter.AddMovieToDb(id);
        }

        var film = _db.Films.Where(movie => movie.Id == id)
            .Include(movie => movie.Filmgenres)
            .ThenInclude(movieGenre => movieGenre.Genre)
            .Include(movie => movie.Filmpeople)
            .ThenInclude(t => t.Person)
            .Include(movie => movie.Filmpeople)
            .ThenInclude(t => t.PersonType)
            .FirstOrDefault();

        if (film != null)
            return new DtoMovie(film);

        return NotFound();
    }

    private bool MovieExists(int id)
    {
        return _db.Films.AsNoTracking().Any(x => x.Id == id);
    }
}