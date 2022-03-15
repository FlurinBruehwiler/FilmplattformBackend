using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;
using WebAPITest.Services;

namespace WebAPITest.Factories;

public class PersonFactory
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly FilmplattformContext _db;
    private readonly PersonService _personService;
    private readonly TmdbService _tmdbService;
    private readonly string _apiKey;

    public PersonFactory(IHttpClientFactory clientFactory, IConfiguration configuration, FilmplattformContext db, 
        PersonService personService, TmdbService tmdbService)
    {
        _clientFactory = clientFactory;
        _db = db;
        _personService = personService;
        _tmdbService = tmdbService;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
    }
    
    
}