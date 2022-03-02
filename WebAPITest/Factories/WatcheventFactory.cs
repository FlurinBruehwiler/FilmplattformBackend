using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Services.UserService;

namespace WebAPITest.Factories;

public class WatcheventFactory
{
    private readonly UserService _userService;

    public WatcheventFactory(UserService userService)
    {
        _userService = userService;
    }
    
    public DtoWatchevent CreateDtoWatchevent(Watchevent watchevent)
    {
        var dtoWatchevent = new DtoWatchevent
        {
            Rating = watchevent.Rating,
            WatchDate = watchevent.Date,
            CreatorId = watchevent.MemberId,
            FilmId = watchevent.FilmId,
            Text = watchevent.Text
        };

        return dtoWatchevent;
    }
    
    public Watchevent CreateWatchevent(DtoWatchevent dtoWatchevent)
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