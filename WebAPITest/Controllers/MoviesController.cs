using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Extensions;
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
    private readonly MovieActions _movieActions;
    private readonly WatcheventFactory _watcheventFactory;
    private readonly string _apiKey;
    private readonly MovieImporter _movieImporter;

    public MoviesController(FilmplattformContext db, IHttpClientFactory clientFactory, 
        IConfiguration configuration, IUserService userService, DtoMovieFactory dtoMovieFactory,
        MovieActions movieActions, WatcheventFactory watcheventFactory)
    {
        _db = db;
        _clientFactory = clientFactory;
        _userService = userService;
        _dtoMovieFactory = dtoMovieFactory;
        _movieActions = movieActions;
        _watcheventFactory = watcheventFactory;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
        _movieImporter = new MovieImporter(clientFactory, configuration, db);
    }
    
    [HttpGet("SearchMovies/{searchString}"), Authorize]
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

    [HttpGet("GetMovieDetails/{id}")]
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
    
    [HttpGet("GetPersonalMovieDetails/{id}"), Authorize]
    public async Task<ActionResult<DtoMovie>> GetMoviePersonal(int id)
    {
        if (!MovieExistsOnDb(id))
        {
            return NotFound();
        }

        var movie = _dtoMovieFactory.GetDtoMoviePersonal(id);

        if (movie is null)
            return NotFound();

        return Ok(movie);
    }
    
    [HttpPatch("PatchLike/{movieId}"), Authorize]
    public async Task<ActionResult> PatchMovieLike(int movieId, bool like)
    {
        if (!await _movieActions.Like(movieId, like))
            return NotFound();

        return Ok();
    }
    
    [HttpPatch("PatchWatchlist/{movieId}"), Authorize]
    public async Task<ActionResult> PatchMovieWatchlist(int movieId, bool watchlist)
    {
        if (!await _movieActions.Watchlist(movieId, watchlist))
            return NotFound();
        
        return Ok();
    }
    
    [HttpPost("Watchevent"), Authorize]
    public async Task<ActionResult> PostWatchevent(DtoWatchevent dtoWatchevent)
    {
        var watchevent = _watcheventFactory.CreateWatchevent(dtoWatchevent);

        var movie = await _db.Films.FirstOrDefaultAsync(x => x.Id == dtoWatchevent.FilmId);

        if (movie is null)
            return NotFound();
        
        movie.Watchevents.Add(watchevent);

        await _db.SaveChangesAsync();
        
        return Ok(watchevent.Id);
    }

    private bool MovieExistsOnDb(int id)
    {
        return _db.Films.AsNoTracking().Any(x => x.Id == id);
    }
}