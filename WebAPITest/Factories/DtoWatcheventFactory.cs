using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;

namespace WebAPITest.Factories;

public class DtoWatcheventFactory
{
    public DtoWatchevent GetDtoWatchevent(Watchevent watchevent)
    {
        var dtoWatchevent = new DtoWatchevent
        {
            Rating = watchevent.Rating,
            WatchDate = watchevent.Date,
            CreatorId = watchevent.MemberId
        };

        return dtoWatchevent;
    }
}