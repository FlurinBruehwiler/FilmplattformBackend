using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;

namespace WebAPITest.Services;

public class MovieService
{
    private readonly FilmplattformContext _db;
    private readonly UserService _userService;

    public MovieService(FilmplattformContext db, UserService userService)
    {
        _db = db;
        _userService = userService;
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