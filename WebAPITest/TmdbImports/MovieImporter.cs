using System.Globalization;
using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;

namespace WebAPITest.TmdbImports;

public class MovieImporter
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly filmplattformContext _db;
    private readonly string _apiKey;
    private readonly GenreImporter _genreImporter;
    private readonly PersonImporter _personImporter;
    private readonly PersonTypeImporter _personTypeImporter;

    public MovieImporter(IHttpClientFactory clientFactory, IConfiguration configuration, filmplattformContext db)
    {
        _clientFactory = clientFactory;
        _db = db;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
        _genreImporter = new GenreImporter(configuration, db);
        _personImporter = new PersonImporter(clientFactory, configuration, db);
        _personTypeImporter = new PersonTypeImporter(db);
    }
    
    public async Task AddMovieToDb(int id)
    {
        if(MovieExists(id))
            return;

        var tmdbMovie = await GetMovieDetailsFromTmdb(id);

        var genres = _genreImporter.GetGenresForMovie(tmdbMovie);
        var people = await _personImporter.GetPeopleForMovie(tmdbMovie);
        
        var film = new Film
        {
            Id = tmdbMovie.Id,
            Title = tmdbMovie.Title,
            ShortDescription = tmdbMovie.ShortDescription,
            LongDescription = tmdbMovie.LongDescription,
            ReleaseDate = DateTime.Parse(tmdbMovie.ReleaseDate ?? DateTime.Now.ToString(CultureInfo.InvariantCulture)),
        };
        
        AddGenresToMovie(genres, film);
        AddPersonToMovie(people, film);

        _db.Films.Add(film);
        await _db.SaveChangesAsync();
    }

    private async Task<TMDBMovieDetails> GetMovieDetailsFromTmdb(int id)
    {
        var url = $"movie/{id}?api_key={_apiKey}";

        TMDBMovieDetails? tmdbMovie;
        var client = _clientFactory.CreateClient("tmdb");

        try
        {
            tmdbMovie = await client.GetFromJsonAsync<TMDBMovieDetails>(url);
        }
        catch (Exception)
        {
            return new TMDBMovieDetails();
        }

        if (tmdbMovie == null)
            return new TMDBMovieDetails();
        
        return tmdbMovie;
    }

    private void AddPersonToMovie(List<(Person, string)>? people, Film film)
    {
        if(people == null)
            return;
        
        foreach (var p in people)
        {
            var person = p.Item1;
            var type = p.Item2;
            
            var personType = _personTypeImporter.GetPersonType(type);
            
            var filmPerson = new Filmperson
            {
                Film = film,
                Person = person,
                PersonType = personType
            };
            
            film.Filmpeople.Add(filmPerson);
        }
    }

    private void AddGenresToMovie(List<Genre> genres, Film film)
    {
        foreach (var genre in genres)
        {
            var filmGenre = new Filmgenre
            {
                Film = film,
                Genre = genre
            };
            
            film.Filmgenres.Add(filmGenre);
        }
    }
    
    private bool MovieExists(int id)
    {
        return _db.Films.Any(x => x.Id == id);
    }
}