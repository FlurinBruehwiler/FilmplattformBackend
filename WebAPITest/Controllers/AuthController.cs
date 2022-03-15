using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Services;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly FilmplattformContext _db;
    private readonly AuthService _authService;

    public AuthController(FilmplattformContext db, AuthService authService)
    {
        _db = db;
        _authService = authService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(DtoUser user)
    {
        _authService.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

        if (_db.Members.Any(x => x.Username == user.Username))
            return BadRequest("User with this Username already Exists");
        
        _db.Members.Add(new Member
        {
            Username = user.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Email = user.Email,
            Vorname = user.Vorname,
            Name = user.Name
        });
        await _db.SaveChangesAsync();

        var member = _db.Members.FirstOrDefault(x => x.Username == user.Username);
        
        if(member != null)
            return Ok(_authService.CreateToken(member));

        return StatusCode(500);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(DtoLogin request)
    {
        var member = _db.Members.AsNoTracking().FirstOrDefault(x => x.Username == request.Username);
        
        if (member == null)
        {
            return BadRequest("User not found");
        }
        
        if (!_authService.VerifyPasswordHash(request.Password, member.PasswordHash, member.PasswordSalt))
        {
            return BadRequest("Wrong Password");
        }

        var token = _authService.CreateToken(member);
        return Ok(token);
    }
}