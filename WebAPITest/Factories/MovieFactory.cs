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
    private readonly GenreService _genreService;
    private readonly MovieService _movieService;
    private readonly PersonService _personService;

    public MovieFactory(FilmplattformContext db, TmdbService tmdbService,
        GenreService genreService, MovieService movieService, PersonService personService)
    {
        _db = db;
        _tmdbService = tmdbService;
        _genreService = genreService;
        _movieService = movieService;
        _personService = personService;
    }
    
    public async Task<bool> CreateMovie(int id)
    {
        if(MovieExists(id))
            return true;

        var tmdbMovie = await _tmdbService.GetMovieDetails(id);

        if (tmdbMovie is null)
            return false;

        var genres = _genreService.GetGenresForMovie(tmdbMovie);
        var people = await _personService.CreatePeopleForMovie(tmdbMovie);
        
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

        _movieService.AddGenres(genres, film);
        _movieService.AddPersonToMovie(people, film);
        
        _db.Films.Add(film);

        await _db.SaveChangesAsync();

        return true;
    }

    private bool MovieExists(int id)
    {
        return _db.Films.AsNoTracking().Any(x => x.Id == id);
    }
}