using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;

namespace WebAPITest.Factories;

public class PersonTypeFactory
{
    private readonly FilmplattformContext _db;

    public PersonTypeFactory(FilmplattformContext db)
    {
        _db = db;
    }

    public Persontype GetPersonType(string name)
    {
        if (PersonTypeExits(name))
            return _db.Persontypes.First(x => x.Name == name);

        var personType = new Persontype
        {
            Name = name
        };

        _db.Persontypes.Add(personType);
        _db.SaveChanges();

        return personType;
    }

    private bool PersonTypeExits(string name)
    {
        return _db.Persontypes.AsNoTracking().Any(x => x.Name == name);
    }
}