namespace WebAPITest.Models.DTO;

public class DtoMoviePersonal
{
    public bool Like { get; set; }
    public bool Watchlist { get; set; }
    public List<DtoList> AvailableLists { get; set; } = new();
    public List<DtoWatcheventDetails> Watchevents { get; set; } = new();
}