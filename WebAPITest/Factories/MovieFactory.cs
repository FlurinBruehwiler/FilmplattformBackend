using System.Globalization;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;
using WebAPITest.Services;

namespace WebAPITest.Factories;

public class MovieFactory
{
    private readonly FilmplattformContext _db;
    private readonly TmdbService _tmdbService;
    private readonly PersonFactory _personFactory;
    private readonly PersonTypeFactory _personTypeFactory;
    private readonly GenreService _genreService;

    public MovieFactory(FilmplattformContext db, TmdbService tmdbService, 
        PersonFactory personFactory, PersonTypeFactory personTypeFactory,
        GenreService genreService)
    {
        _db = db;
        _tmdbService = tmdbService;
        _personFactory = personFactory;
        _personTypeFactory = personTypeFactory;
        _genreService = genreService;
    }
    
    public async Task<bool> CreateMovie(int id)
    {
        if(MovieExists(id))
            return true;

        var tmdbMovie = await _tmdbService.GetMovieDetails(id);

        if (tmdbMovie is null)
            return false;

        var genres = _genreService.GetGenresForMovie(tmdbMovie);
        var people = await _personFactory.GetPeopleForMovie(tmdbMovie);
        
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

        return true;
    }

    private void AddPersonToMovie(List<(Person Person, string Departement)>? people, Film film)
    {
        if(people == null)
            return;
        
        foreach (var p in people)
        {
            var person = p.Person;
            var personType = _personTypeFactory.GetPersonType(p.Departement);

            if(film.Filmpeople.Any(f => f.Film.Id == film.Id &&
                                        f.Person.Id == person.Id &&
                                        f.PersonType.Id == personType.Id))
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

    private bool MovieExists(int id)
    {
        return _db.Films.AsNoTracking().Any(x => x.Id == id);
    }
}