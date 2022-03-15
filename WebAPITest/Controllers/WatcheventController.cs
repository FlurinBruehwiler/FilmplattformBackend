using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Factories;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Services;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class WatcheventController : ControllerBase
{
    private readonly WatcheventFactory _watcheventFactory;
    private readonly FilmplattformContext _db;
    private readonly MovieService _movieService;

    public WatcheventController(WatcheventFactory watcheventFactory, FilmplattformContext db, MovieService movieService)
    {
        _watcheventFactory = watcheventFactory;
        _db = db;
        _movieService = movieService;
    }
    
    [HttpPost, Authorize]
    public async Task<ActionResult> PostWatchevent(DtoPostWatchevent dtoWatchevent)
    {
        var watchevent = _watcheventFactory.CreateWatchevent(dtoWatchevent);

        var movie = await _db.Films.FirstOrDefaultAsync(x => x.Id == dtoWatchevent.FilmId);

        if (movie is null)
            return NotFound();
        
        movie.Watchevents.Add(watchevent);

        await _db.SaveChangesAsync();
        
        return Ok(watchevent.Id);
    }
    
    [HttpGet("GetReviews/{movieId}")]
    public async Task<ActionResult<DtoWatchevents>> GetWatchevents(int movieId)
    {
        var movie = _movieService.GetMovieById(movieId);
        
        if (movie == null)
            return NotFound($"No movie with id {movieId} found");

        var watchEvents = _db.Watchevents.Where(x => x.FilmId == movieId && x.Text != null);
        var list = watchEvents.ToArray().ToList();
        var dtoWatchEvents = list.Select(x => _watcheventFactory.CreateDtoWatchevent(x)).ToList();

        return Ok(new DtoWatchevents(movie, _movieService.GetDirector(movieId) ?? string.Empty, dtoWatchEvents));
    }
}