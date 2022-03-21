namespace WebAPITest.Models.DTO;

public class DtoMovies
{
    public List<DtoWatcheventDetails> Watchevents { get; set; } = new List<DtoWatcheventDetails>();

    public List<DtoMovie> LikedMovies { get; set; } = new List<DtoMovie>();
}