using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Services.UserService;

namespace WebAPITest.Factories;

public class DtoMovieFactory
{
    private readonly FilmplattformContext _db;
    private readonly IUserService _userService;
    private readonly DtoWatcheventFactory _dtoWatcheventFactory;

    public DtoMovieFactory(FilmplattformContext db, IUserService userService, DtoWatcheventFactory dtoWatcheventFactory)
    {
        _db = db;
        _userService = userService;
        _dtoWatcheventFactory = dtoWatcheventFactory;
    }
    
    public DtoMovie? GetDtoMovie(int movieId)
    {
        var film = _db.Films.Where(movie => movie.Id == movieId)
            .Include(movie => movie.Filmgenres)
            .ThenInclude(movieGenre => movieGenre.Genre)
            .Include(movie => movie.Filmpeople)
            .ThenInclude(t => t.Person)
            .Include(movie => movie.Filmpeople)
            .ThenInclude(t => t.PersonType)
            .FirstOrDefault();
        
        Console.WriteLine($"The User Id is {_userService.GetId()}");

        if (film == null)
        {
            return null;
        }
        
        return new DtoMovie(film);
    }
    
    public DtoMoviePersonal? GetDtoMoviePersonal(int movieId)
    {
        var user = _db.Members.Where(member => member.Id == _userService.GetId())
            .Include(member => member.Watchevents
                .Where(watchEvent => watchEvent.FilmId == movieId))
            .Include(member => member.Filmmembers
                .Where(filmMember => filmMember.FilmId == movieId))
            .Include(member => member.Lists)
            .FirstOrDefault();

        if (user == null)
        {
            return null;
        }

        var filmMember = user.Filmmembers.FirstOrDefault(filmMember => filmMember.FilmId == movieId);
        
        return new DtoMoviePersonal
        {
            Like = HasLike(filmMember),
            Watchlist = HasWatchlist(filmMember),
            Watchevents = GetWatchEvents(user, movieId)
        };
    }

    private List<DtoWatchevent> GetWatchEvents(Member user, int movieId)
    {
        return user.Watchevents.Where(x => x.FilmId == movieId)
            .Select(watchEvent => _dtoWatcheventFactory.GetDtoWatchevent(watchEvent)).ToList();
    }

    private bool HasWatchlist(Filmmember? movieUser)
    {
        return movieUser?.WatchlistBool ?? false;
    }

    private bool HasLike(Filmmember? movieUser)
    {
        return movieUser?.LikeBool ?? false;
    }
}