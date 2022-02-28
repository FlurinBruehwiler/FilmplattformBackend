using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Factories;
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
    private readonly DtoMovieFactory _dtoMovieFactory;
    private readonly string _apiKey;
    private readonly MovieImporter _movieImporter;

    public MoviesController(FilmplattformContext db, IHttpClientFactory clientFactory, IConfiguration configuration, IUserService userService, DtoMovieFactory dtoMovieFactory)
    {
        _db = db;
        _clientFactory = clientFactory;
        _userService = userService;
        _dtoMovieFactory = dtoMovieFactory;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
        _movieImporter = new MovieImporter(clientFactory, configuration, db);
    }
    
    [HttpGet("SearchMovies/{searchString}")]
    public async Task<ActionResult<TMDBMovieSearchResult>> SearchMovies(string searchString)
    {
        Console.WriteLine(_userService.GetId());
        
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
        if (!MovieExistsOnDb(id))
        {
            if (!await _movieImporter.AddMovieToDb(id))
            {
                return NotFound();
            }
        }

        var movie = _dtoMovieFactory.GetDtoMovie(id);

        if (movie is null)
            return NotFound();

        return Ok(movie);
    }

    private bool MovieExistsOnDb(int id)
    {
        return _db.Films.AsNoTracking().Any(x => x.Id == id);
    }
}