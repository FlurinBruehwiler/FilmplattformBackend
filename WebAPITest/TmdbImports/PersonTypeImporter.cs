using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;

namespace WebAPITest.TmdbImports;

public class PersonTypeImporter
{
    private readonly FilmplattformContext _db;

    public PersonTypeImporter(FilmplattformContext db)
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

        return personType;
    }

    private bool PersonTypeExits(string name)
    {
        return _db.Persontypes.AsNoTracking().Any(x => x.Name == name);
    }
}