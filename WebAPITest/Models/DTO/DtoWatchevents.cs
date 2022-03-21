using WebAPITest.Models.DB;

namespace WebAPITest.Models.DTO;

public class DtoWatchevents
{
    public DtoWatchevents(Film movie, string directorName, List<DtoWatcheventDetails> watchevents)
    {
        FilmId = movie.Id;
        FilmTitle = movie.Title;
        ReleaseDate = movie.ReleaseDate;
        DirectorName = directorName;
        Watchevents = watchevents;
        PosterUrl = movie.PosterUrl;
        BackdropUrl = movie.BackdropUrl;
    }
    
    public int FilmId { get; set; }
    public string FilmTitle { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string DirectorName { get; set; }
    public string PosterUrl { get; set; }
    public string BackdropUrl { get; set; }
    public List<DtoWatcheventDetails> Watchevents { get; set; }
}