using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Factories;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Services.MovieService;
using WebAPITest.TmdbImports;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly FilmplattformContext _db;
    private readonly DtoMovieFactory _dtoMovieFactory;
    private readonly IMovieService _movieService;
    private readonly MovieImporter _movieImporter;

    public MoviesController(FilmplattformContext db, IHttpClientFactory clientFactory, 
        IConfiguration configuration, DtoMovieFactory dtoMovieFactory, IMovieService movieService)
    {
        _db = db;
        _dtoMovieFactory = dtoMovieFactory;
        _movieService = movieService;
        _movieImporter = new MovieImporter(clientFactory, configuration, db);
    }

    [HttpGet("GetMovieDetails/{id}")]
    public async Task<ActionResult<DtoMovieDetails>> GetMovie(int id)
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
    public async Task<ActionResult<DtoMovieDetails>> GetMoviePersonal(int id)
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
        if (!await _movieService.PatchLike(movieId, like))
            return NotFound();

        return Ok();
    }
    
    [HttpPatch("PatchWatchlist/{movieId}"), Authorize]
    public async Task<ActionResult> PatchMovieWatchlist(int movieId, bool watchlist)
    {
        if (!await _movieService.PatchWatchlist(movieId, watchlist))
            return NotFound();
        
        return Ok();
    }

    private bool MovieExistsOnDb(int id)
    {
        return _db.Films.AsNoTracking().Any(x => x.Id == id);
    }
}