namespace WebAPITest.Models.DTO;

public class DtoPostWatchevent
{
    public DateTime WatchDate { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
    public int FilmId { get; set; }
}