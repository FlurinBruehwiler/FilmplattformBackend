using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;
using WebAPITest.Services.UserService;

namespace WebAPITest.Actions;

public class MovieActions
{
    private readonly FilmplattformContext _db;
    private readonly IUserService _userService;

    public MovieActions(FilmplattformContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }
    
    public async Task<bool> Like(int movieId, bool like)
    {
        var movie = GetMovieWithFilmMember(movieId);

        if (movie is null)
            return false;

        movie.Filmmembers.First().LikeBool = like;
        
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Watchlist(int movieId, bool watchlist)
    {
        var movie = GetMovieWithFilmMember(movieId);

        if (movie is null)
            return false;
        
        movie.Filmmembers.First().WatchlistBool = watchlist;
        
        await _db.SaveChangesAsync();

        return true;
    }

    public void CreateWatchEvent(int movieId)
    {
        
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