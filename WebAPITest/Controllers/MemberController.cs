using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Services;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberController : ControllerBase
{
    private readonly FilmplattformContext _db;
    private readonly UserService _userService;

    public MemberController(FilmplattformContext db, UserService userService)
    {
        _db = db;
        _userService = userService;
    }
    
    [HttpGet("{id}/Movies")]
    public async Task<ActionResult<List<DtoMovieWithWatchevents>>> GetMovies(int id)
    {
        var user = _db.Members.Where(x => x.Id == id)
            .Include(x => x.Watchevents)
            .ThenInclude(x => x.Film)
            .FirstOrDefault();

        if (user is null)
            return NotFound();
        List<DtoMovieWithWatchevents> output = new();

        foreach (var watchevent in user.Watchevents)
        {
            var dtoMovieWithWatchevent = output.FirstOrDefault(x => x.Id == watchevent.FilmId);
            
            if (dtoMovieWithWatchevent is not null)
            {
                dtoMovieWithWatchevent.Ratings.Add(watchevent.Rating ?? 0);
            }
            else
            {
                output.Add(new DtoMovieWithWatchevents
                {
                    Id = watchevent.FilmId,
                    Ratings = new List<int>{watchevent.Rating},
                    Title = watchevent.Film.Title,
                    MoviePoster = watchevent.Film.PosterUrl,
                    ReleaseDate = watchevent.Film.ReleaseDate,
                    LastTimeWatched = watchevent.Film.
                    
                    
                });
            }
            
            
        }

        return Ok(output);
    }
    
    [HttpGet("{id}/Followers")]
    public async Task<ActionResult<List<DtoMember>>> GetFollowers(int id)
    {
        return Ok(_db.Followings.Where(x => x.FollowingId == id).Select(x => new DtoMember
        {
            Id = x.Follower.Id,
            Username = x.Follower.Name,
            ProfilePicturePath = x.Follower.Email
        }).ToList());
    }
    
    [HttpGet("{id}/Following")]
    public async Task<ActionResult<List<DtoMember>>> GetFollowing(int id)
    {
        return Ok(_db.Followings.Where(x => x.FollowerId == id).Select(x => new DtoMember
        {
            Id = x.FollowingNavigation.Id,
            Username = x.FollowingNavigation.Name,
            ProfilePicturePath = x.FollowingNavigation.Email
        }).ToList());
    }
}