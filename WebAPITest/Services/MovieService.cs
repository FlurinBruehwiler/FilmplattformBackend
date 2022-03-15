using Microsoft.EntityFrameworkCore;
using WebAPITest.Factories;
using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;

namespace WebAPITest.Services;

public class MovieService
{
    private readonly FilmplattformContext _db;
    private readonly UserService _userService;
    private readonly PersonTypeFactory _personTypeFactory;

    public MovieService(FilmplattformContext db, UserService userService, PersonTypeFactory personTypeFactory)
    {
        _db = db;
        _userService = userService;
        _personTypeFactory = personTypeFactory;
    }

    public async Task<bool> PatchLike(int movieId, bool like)
    {
        var movie = GetMovieWithFilmMember(movieId);

        if (movie is null)
            return false;

        movie.Filmmembers.First().LikeBool = like;
        
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> PatchWatchlist(int movieId, bool watchlist)
    {
        var movie = GetMovieWithFilmMember(movieId);

        if (movie is null)
            return false;
        
        movie.Filmmembers.First().WatchlistBool = watchlist;
        
        await _db.SaveChangesAsync();

        return true;
    }

    public Film? GetMovieById(int id)
    {
        return _db.Films.FirstOrDefault(x => x.Id == id);
    }

    public string? GetDirector(int id)
    {
        var movie = _db.Films.Where(x => x.Id == id)
            .Include(x => x.Filmpeople)
            .ThenInclude(x => x.PersonType)
            .Include(x => x.Filmpeople)
            .ThenInclude(x => x.Person).FirstOrDefault();

        return movie?.Filmpeople.FirstOrDefault(x => x.PersonType.Name == "Directing")?.Person.Name;
    }
    
    public void AddGenres(List<Genre> genres, Film film)
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
    
    public void AddPersonToMovie(List<(Person Person, string Departement)>? people, Film film)
    {
        if(people == null)
            return;
        
        foreach (var p in people)
        {
            var person = p.Person;
            var personType = _personTypeFactory.GetPersonType(p.Departement);

            if (film.Filmpeople.Any(f => f.Film.Id == film.Id &&
                                         f.Person.Id == person.Id &&
                                         f.PersonType.Id == personType.Id))
                return;
            
            var filmPerson = new Filmperson
            {
                Film = film,
                Person = person,
                PersonType = personType
            };

            film.Filmpeople.Add(filmPerson);
        }
    }

    private Film? GetMovieWithFilmMember(int movieId)
    {
        var movie = _db.Films.Where(x => x.Id == movieId)
            .Include(x => x.Filmmembers
                .Where(t => t.MemberId == _userService.GetId()))
            .FirstOrDefault();
        
        if (movie is null)
            return null;

        if (movie.Filmmembers.Count == 0)
        {
            CreateFilmMember(movie);
        }

        return movie;
    }
    
    private void CreateFilmMember(Film movie)
    {
        movie.Filmmembers.Add(new Filmmember
        {
            Film = movie,
            MemberId = _userService.GetId(),
        });
    }
}