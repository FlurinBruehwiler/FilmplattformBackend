using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ActionResult<List<DtoMovie>>> GetMovies(int id)
    {
        var user = _db.Members.Where(x => x.Id == id)
            .Include(x => x.Filmmembers)
            .ThenInclude(x => x.Film)
            .Include(x => x.Watchevents)
            .ThenInclude(x => x.Film)
            .FirstOrDefault();

        if (user is null)
            return NotFound();

        return Ok(user.Filmmembers.Where(x => x.LikeBool).Select(x => new DtoMovie
        {
            Id = x.Film.Id,
            Title = x.Film.Title,
            PosterPath = x.Film.PosterUrl
        }).Concat(user.Watchevents.Select(x => new DtoMovie
        {
            Id = x.Film.Id,
            Title = x.Film.Title,
            PosterPath = x.Film.PosterUrl
        })).ToList());
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