using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Services;

namespace WebAPITest.Factories;

public class WatcheventFactory
{
    private readonly UserService _userService;
    private readonly FilmplattformContext _db;

    public WatcheventFactory(UserService userService, FilmplattformContext db)
    {
        _userService = userService;
        _db = db;
    }
    
    public DtoWatchevent CreateDtoWatchevent(Watchevent watchevent)
    {
        var user = _userService.GetUserById(watchevent.MemberId);
        
        var dtoWatchevent = new DtoWatchevent
        {
            Id = watchevent.Id,
            Rating = watchevent.Rating,
            WatchDate = watchevent.Date,
            CreatorId = watchevent.MemberId,
            CreatorName = $"{user?.Vorname ?? string.Empty} {user?.Name ?? string.Empty}",
            FilmId = watchevent.FilmId,
            Text = watchevent.Text
        };

        return dtoWatchevent;
    }
    
    public Watchevent CreateWatchevent(DtoPostWatchevent dtoWatchevent)
    {
        var watchevent = new Watchevent
        {
            Date = dtoWatchevent.WatchDate,
            Rating = dtoWatchevent.Rating,
            Text = dtoWatchevent.Text,
            MemberId = _userService.GetId(),
        };

        return watchevent;
    }
}