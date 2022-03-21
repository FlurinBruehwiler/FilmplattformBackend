namespace WebAPITest.Models.DTO;

public class DtoWatcheventDetails
{
    public int Id { get; set; }
    public DateTime WatchDate { get; set; }
    public int? Rating { get; set; }
    public string Text { get; set; } = string.Empty;
    public int CreatorId { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public int FilmId { get; set; }
}