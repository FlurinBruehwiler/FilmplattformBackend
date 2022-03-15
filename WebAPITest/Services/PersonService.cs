using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;

namespace WebAPITest.Services;

public class PersonService
{
    private readonly FilmplattformContext _db;
    private readonly TmdbService _tmdbService;

    public PersonService(FilmplattformContext db, TmdbService tmdbService)
    {
        _db = db;
        _tmdbService = tmdbService;
    }
    
    public Person? GetPersonById(int id)
    {
        return _db.People.FirstOrDefault(x => x.Id == id);
    }
    
    public List<TMDBPerson> CombineCrewAndCast(List<TmdbMovieCrew>? crew, List<TmdbMovieCast>? cast)
    {
        if (cast == null)
            return new List<TMDBPerson>();
        
        var tmdbPeople = cast.Cast<TMDBPerson>().ToList();
        
        if(crew != null)
            tmdbPeople.AddRange(crew);

        return tmdbPeople;
    }
    
    public Person? GetPersonIfExists(int id, List<(Person Person, string Departement)> addedPeople)
    {
        if (_db.People.Any(x => x.Id == id))
        {
            return _db.People.First(x => x.Id == id);
        }

        if (addedPeople.Any(person => person.Person.Id == id))
        {
            return addedPeople.First(tuple => tuple.Person.Id == id).Person;
        }

        return null;
    }
    
    public async Task<List<(Person, string)>> CreatePeopleForMovie(TMDBMovieDetails tmdbMovie)
    {
        var id = tmdbMovie.Id;
        
        var credits = await _tmdbService.GetPeople(id);

        var tmdbPeople = CombineCrewAndCast(credits.Crew, credits.Cast);

        List<(Person, string)> people = new();
        
        foreach (var tmdbPerson in tmdbPeople)
        {
            var person = GetPersonIfExists(tmdbPerson.Id, people);

            if(person == null)
            {
                person = new Person
                {
                    Id = tmdbPerson.Id,
                    Name = tmdbPerson.Name
                };
            }

            var department = tmdbPerson is TmdbMovieCrew crew ? crew.Department : "Actor";

            if(department != null)
                people.Add((person, department));
        }

        return people;
    }
}