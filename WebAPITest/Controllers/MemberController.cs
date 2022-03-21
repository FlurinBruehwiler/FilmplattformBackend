using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class MemberController : ControllerBase
{
    private readonly FilmplattformContext _db;

    public MemberController(FilmplattformContext db)
    {
        _db = db;
    }
    
    [HttpGet("{id}/Movies")]
    public ActionResult<List<DtoMovieWithRating>> GetMovies(int id)
    {
        var user = _db.Members.Where(x => x.Id == id)
            .Include(x => x.Watchevents)
            .ThenInclude(x => x.Film)
            .FirstOrDefault();

        if (user is null)
            return NotFound();
        
        var result = user.Watchevents.GroupBy(x => x.FilmId).Select(watchEvents =>
        {
            var watchEvent = watchEvents.First();
            return new DtoMovieWithRating
            {
                Id = watchEvent.FilmId,
                Ratings = watchEvents.Where(x => x.Rating != 0).Select(x => x.Rating).ToList(),
                Title = watchEvent.Film.Title,
                MoviePoster = watchEvent.Film.PosterUrl,
                ReleaseDate = watchEvent.Film.ReleaseDate,
                LastTimeWatched = watchEvents.OrderByDescending(x => x.Date).First().Date
            };
        }).ToList();

        return Ok(result);
    }
    
    [HttpGet("{id}/Followers")]
    public ActionResult<List<DtoMember>> GetFollowers(int id)
    {
        return Ok(_db.Followings.Where(x => x.FollowingId == id).Select(x => new DtoMember
        {
            Id = x.Follower.Id,
            Username = x.Follower.Name,
            ProfilePicturePath = x.Follower.Email
        }).ToList());
    }
    
    [HttpGet("{id}/Following")]
    public ActionResult<List<DtoMember>> GetFollowing(int id)
    {
        return Ok(_db.Followings.Where(x => x.FollowerId == id).Select(x => new DtoMember
        {
            Id = x.FollowingNavigation.Id,
            Username = x.FollowingNavigation.Name,
            ProfilePicturePath = x.FollowingNavigation.Email
        }).ToList());
    }
}