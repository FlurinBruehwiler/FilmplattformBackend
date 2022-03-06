using WebAPITest.Models.DB;

namespace WebAPITest.Services.PersonService;

public class PersonService : IPersonService
{
    private readonly FilmplattformContext _db;

    public PersonService(FilmplattformContext db)
    {
        _db = db;
    }
    
    public Person? GetPersonById(int id)
    {
        return _db.People.FirstOrDefault(x => x.Id == id);
    }
}