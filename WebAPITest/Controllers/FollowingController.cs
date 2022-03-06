using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models.DB;
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
    
    [HttpPatch("Follow/{id}"), Authorize]
    public async Task<ActionResult> Follow(int id, bool follow)
    {
        //ToDo RemoveFollow
        
        var currentUser = _userService.GetUser();
        var userToFollow = _userService.GetUserById(id);

        if (userToFollow is null)
            return NotFound($"No user with the id {id} found");

        if (currentUser.Id == userToFollow.Id)
            return BadRequest($"You cannot follow yourself");

        if (_db.Followings.Any(x => x.FollowerId == currentUser.Id && x.FollowingId == userToFollow.Id))
            return BadRequest("You are already following this user");
        
        currentUser.FollowingFollowers.Add(new Following
        {
            Follower = currentUser,
            FollowingNavigation = userToFollow
        });

        await _db.SaveChangesAsync();
        
        return Ok();
    }
}