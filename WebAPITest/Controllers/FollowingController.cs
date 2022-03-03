using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Services.UserService;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class FollowingController : ControllerBase
{
    private readonly FilmplattformContext _db;
    private readonly IUserService _userService;

    public FollowingController(FilmplattformContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }
    
    [HttpGet("Follow/{id}"), Authorize]
    public async Task<ActionResult<DtoMovie>> Follow(int id)
    {
        var currentUser = _userService.GetUser();
        var userToFollow = _userService.GetUserById(id);

        if (userToFollow is null)
            return NotFound($"No user with the id {id} found");
        
        currentUser.FollowingFollowingNavigations.Add(new Following
        {
            Follower = currentUser,
            FollowingNavigation = userToFollow
        });

        await _db.SaveChangesAsync();
        
        return Ok();
    }
}