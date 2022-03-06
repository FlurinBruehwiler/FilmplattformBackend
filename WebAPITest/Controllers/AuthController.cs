using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using static System.Text.Encoding;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly FilmplattformContext _db;
    private readonly IConfiguration _configuration;

    public AuthController(FilmplattformContext db, IConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<Member>> Register(DtoUser user)
    {
        CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

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

        return Ok("Benuter Erstellt");
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(DtoUser request)
    {
        var member = _db.Members.AsNoTracking().FirstOrDefault(x => x.Username == request.Username);
        
        if (member == null)
        {
            return BadRequest("User not found");
        }
        
        if (!VerifyPasswordHash(request.Password, member.PasswordHash, member.PasswordSalt))
        {
            return BadRequest("Wrong Password");
        }

        var token = CreateToken(member);
        return Ok(token);
    }

    private string CreateToken(Member member)
    {
        List<Claim> claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, member.Id.ToString())
        };

        var key = new SymmetricSecurityKey(UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(100),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        
        return jwt;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}