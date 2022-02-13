using WebAPITest.Models.DB;

namespace WebAPITest.TmdbImports;

public class PersonTypeImporter
{
    private readonly filmplattformContext _db;

    public PersonTypeImporter(filmplattformContext db)
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

        return personType;
    }

    private bool PersonTypeExits(string name)
    {
        return _db.Persontypes.Any(x => x.Name == name);
    }
}