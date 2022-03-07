using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Services;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class ListController : ControllerBase
{
    private readonly FilmplattformContext _db;
    private readonly UserService _userService;

    public ListController(FilmplattformContext db, UserService userService)
    {
        _db = db;
        _userService = userService;
    }
    
    [HttpPost("{name}"), Authorize]
    public async Task<ActionResult> PostList(string name)
    {
        var list = new List
        {
            MemberId = _userService.GetId(),
            Name = name
        };
        
        _db.Lists.Add(list);

        await _db.SaveChangesAsync();

        return Ok(list.Id);
    }
    
    [HttpPost("AddMovie"), Authorize]
    public async Task<ActionResult> PostMovieToList(ListMovie listMovie)
    {
        var movie = _db.Films.FirstOrDefault(x => x.Id == listMovie.MovieId);

        if (movie is null)
            return NotFound($"There is no movie with the id {listMovie.MovieId}");

        var list = _db.Lists.FirstOrDefault(x => x.Id == listMovie.ListId);

        if (list is null)
            return NotFound($"There is no list with the id {listMovie.ListId}");

        if (list.MemberId != _userService.GetId())
            return Unauthorized($"The logged in user is not the creator of the list with the id {listMovie.ListId}");
        
        list.Listfilms.Add(new Listfilm
        {
            Film = movie,
            List = list
        });

        await _db.SaveChangesAsync();
        
        return Ok();
    }
}