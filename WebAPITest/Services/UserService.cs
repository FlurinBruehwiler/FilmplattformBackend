using System.Security.Claims;
using WebAPITest.Models.DB;

namespace WebAPITest.Services;

public class UserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly FilmplattformContext _db;

    public UserService(IHttpContextAccessor httpContextAccessor, FilmplattformContext db)
    {
        _httpContextAccessor = httpContextAccessor;
        _db = db;
    }
    
    public int GetId()
    {
        if (_httpContextAccessor.HttpContext == null)
            return -1;
            
        var idString = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (int.TryParse(idString, out int id))
        {
            return id;
        }

        return -1;
    }

    public Member GetUser()
    {
        return _db.Members.FirstOrDefault(x => x.Id == GetId()) ?? throw new Exception();
    }

    public Member? GetUserById(int id)
    {
        return _db.Members.FirstOrDefault(x => x.Id == id);
    }
}