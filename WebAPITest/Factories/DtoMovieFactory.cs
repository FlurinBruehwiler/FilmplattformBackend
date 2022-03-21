using K4os.Compression.LZ4.Internal;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Services;

namespace WebAPITest.Factories;

public class DtoMovieFactory
{
    private readonly FilmplattformContext _db;
    private readonly UserService _userService;
    private readonly WatcheventFactory _watcheventFactory;

    public DtoMovieFactory(FilmplattformContext db, UserService userService, WatcheventFactory watcheventFactory)
    {
        _db = db;
        _userService = userService;
        _watcheventFactory = watcheventFactory;
    }
    
    public DtoMovieDetails? GetDtoMovie(int movieId)
    {
        var film = _db.Films.Where(movie => movie.Id == movieId)
            .Include(movie => movie.Filmgenres)
            .ThenInclude(movieGenre => movieGenre.Genre)
            .Include(movie => movie.Filmpeople)
            .ThenInclude(t => t.Person)
            .Include(movie => movie.Filmpeople)
            .ThenInclude(t => t.PersonType)
            .Include(t => t.Watchevents)
            .FirstOrDefault();
        
        Console.WriteLine($"The User Id is {_userService.GetId()}");

        if (film == null)
        {
            return null;
        }

        var dtoMovie = new DtoMovieDetails
        {
            Id = film.Id,
            Title = film.Title,
            ShortDescription = film.ShortDescription,
            LongDescription = film.LongDescription,
            PosterURL = film.PosterUrl,
            BackdropURL = film.BackdropUrl,
            ReleaseDate = film.ReleaseDate,
            Genres = film.Filmgenres.Select(x => new DtoGenre(x.Genre)).ToList(),
            People = film.Filmpeople.Select(x => new DtoPerson(x)).ToList(),
            AverageRating = film.Watchevents.Where(x => x.Rating != 0).Select(x => x.Rating).Sum()
        };
        
        return dtoMovie;
    }
    
    public DtoMoviePersonal? GetDtoMoviePersonal(int movieId)
    {
        var user = _db.Members.Where(member => member.Id == _userService.GetId())
            .Include(member => member.Watchevents
                .Where(watchEvent => watchEvent.FilmId == movieId))
            .Include(member => member.Filmmembers
                .Where(filmMember => filmMember.FilmId == movieId))
            .Include(member => member.Lists)
            .ThenInclude(list => list.Listfilms)
            .Include(x => x.FollowingFollowingNavigations)
            .ThenInclude(x => x.Follower)
            .ThenInclude(x => x.Watchevents.Where(watchEvent => watchEvent.FilmId == movieId))
            .FirstOrDefault();

        if (user == null)
            return null;
        

        var filmMember = user.Filmmembers.FirstOrDefault(filmMember => filmMember.FilmId == movieId);
        
        return new DtoMoviePersonal
        {
            Like = HasLike(filmMember),
            Watchlist = HasWatchlist(filmMember),
            Watchevents = GetWatchEvents(user, movieId),
            AvailableLists = GetAvailableLists(user, movieId)
        };
    }

    private List<DtoList> GetAvailableLists(Member user, int movieId)
    {
        return user.Lists.Where(list => list.Listfilms.All(x => x.FilmId != movieId))
            .Select(list => new DtoList {Id = list.Id, Name = list.Name}).ToList();
    }

    private List<DtoWatcheventDetails> GetWatchEvents(Member user, int movieId)
    {
        return user.FollowingFollowingNavigations.SelectMany(x => x.Follower.Watchevents.Where(f => f.FilmId == movieId))
            .Concat(user.Watchevents.Where(x => x.FilmId == movieId))
            .Select(watchEvent => _watcheventFactory.CreateDtoWatchevent(watchEvent)).ToList();
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