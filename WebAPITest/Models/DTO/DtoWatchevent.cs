namespace WebAPITest.Models.DTO;

public class DtoWatchevent
{
    public DateTime WatchDate { get; set; }
    public int? Rating { get; set; }
    public string Text { get; set; }
    public int CreatorId { get; set; }
    public string CreatorName { get; set; }
    public int FilmId { get; set; }
}