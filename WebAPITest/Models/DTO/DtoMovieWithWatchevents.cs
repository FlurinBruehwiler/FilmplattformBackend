namespace WebAPITest.Models.DTO;

public class DtoMovieWithWatchevents
{
    public List<int> Ratings { get; set; } = new();
    public DateTime LastTimeWatched { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string MoviePoster { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Id { get; set; }
}