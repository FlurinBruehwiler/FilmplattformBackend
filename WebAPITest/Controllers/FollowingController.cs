using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Services;

namespace WebAPITest.Controllers;

[ApiController]
public class FollowingController : ControllerBase
{
    private readonly FilmplattformContext _db;
    private readonly UserService _userService;
    private readonly FollowService _followService;

    public FollowingController(FilmplattformContext db, UserService userService, FollowService followService)
    {
        _db = db;
        _userService = userService;
        _followService = followService;
    }
    
    [HttpPatch("Follow/{id}"), Authorize]
    public async Task<ActionResult> Follow(int id, bool follow)
    {
        var follower = _userService.GetUser();
        var following = _userService.GetUserById(id);

        if (following is null)
            return NotFound($"No user with the id {id} found");

        (bool success, string errorCode) result;
        
        if (follow)
            result = await _followService.Follow(follower, following);
        else
            result = await _followService.UnFollow(follower, following);

        if (result.success)
            return Ok();

        return NotFound(result.errorCode);
    }
}