using WebAPITest.Models.DB;

namespace WebAPITest.Services.UserService;

public interface IUserService
{
    int GetId();
    Member GetUser();
    Member? GetUserById(int id);
}