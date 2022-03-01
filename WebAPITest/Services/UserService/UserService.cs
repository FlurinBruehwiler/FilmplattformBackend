using System.Security.Claims;

namespace WebAPITest.Services.UserService;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
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
}