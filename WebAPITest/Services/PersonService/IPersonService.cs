using WebAPITest.Models.DB;

namespace WebAPITest.Services.PersonService;

public interface IPersonService
{
    Person? GetPersonById(int id);
}