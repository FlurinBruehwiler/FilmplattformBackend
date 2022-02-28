using System.Configuration;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;

namespace WebAPITest.TmdbImports;

public class MovieImporter
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly FilmplattformContext _db;
    private readonly string _apiKey;
    private readonly GenreImporter _genreImporter;
    private readonly PersonImporter _personImporter;
    private readonly PersonTypeImporter _personTypeImporter;

    public MovieImporter(IHttpClientFactory clientFactory, IConfiguration configuration, FilmplattformContext db)
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
            PosterUrl = tmdbMovie.PosterPath,
            BackdropUrl = tmdbMovie.BackdropPath
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

    private void AddPersonToMovie(List<(Person Person, string Departement)>? people, Film film)
    {
        if(people == null)
            return;
        
        foreach (var p in people)
        {
            var person = p.Person;
            var personType = _personTypeImporter.GetPersonType(p.Departement);

            if(film.Filmpeople.Any(f => f.Film.Id == film.Id && f.Person.Id == person.Id && f.PersonType.Id == personType.Id))
            {
                return;
            }
            
            var filmPerson = new Filmperson
            {
                Film = film,
                Person = person,
                PersonType = personType
            };

            if (person.Id == 947)
            {
                Console.WriteLine("tset");
            }

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

    private bool FilmGenreExists(Film film, Genre genre)
    {
        return _db.Filmgenres.AsNoTracking().Any(x => x.FilmId == film.Id && x.GenreId == genre.Id);
    }
    
    private bool MovieExists(int id)
    {
        return _db.Films.AsNoTracking().Any(x => x.Id == id);
    }
}