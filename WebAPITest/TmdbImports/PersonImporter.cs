using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;

namespace WebAPITest.TmdbImports;

public class PersonImporter
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly FilmplattformContext _db;
    private readonly string _apiKey;

    public PersonImporter(IHttpClientFactory clientFactory, IConfiguration configuration, FilmplattformContext db)
    {
        _clientFactory = clientFactory;
        _db = db;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
    }
    
    public async Task<List<(Person, string)>> GetPeopleForMovie(TMDBMovieDetails tmdbMovie)
    {
        var id = tmdbMovie.Id;
        
        var credits = await GetPeopleFromTmdb(id);

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

            var department = tmdbPerson is TMDBCrew crew ? crew.Department : "Actor";

            if(department != null)
                people.Add((person, department));
        }

        return people;
    }

    private List<TMDBPerson> CombineCrewAndCast(List<TMDBCrew>? crew, List<TMDBCast>? cast)
    {
        if (cast == null)
            return new List<TMDBPerson>();
        
        var tmdbPeople = cast.Cast<TMDBPerson>().ToList();
        
        if(crew != null)
            tmdbPeople.AddRange(crew);

        return tmdbPeople;
    }
    
    private async Task<TMDBCredits> GetPeopleFromTmdb(int id)
    {
        var url = $"movie/{id}/credits?api_key={_apiKey}";
        
        TMDBCredits? tmdbCredits;
        var client = _clientFactory.CreateClient("tmdb");
        
        try
        {
            tmdbCredits = await client.GetFromJsonAsync<TMDBCredits>(url);
            if (tmdbCredits == null)
                throw new Exception();
        }
        catch (Exception)
        {
            return new TMDBCredits();
        }

        return tmdbCredits;
    }

    private Person? GetPersonIfExists(int id, List<(Person Person, string Departement)> addedPeople)
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
}