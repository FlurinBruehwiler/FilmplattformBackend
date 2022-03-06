using WebAPITest.Models.DB;

namespace WebAPITest.Models.DTO;

public class DtoMovieDetails
{
    public string PosterURL { get; set; } = string.Empty;
    public string BackdropURL { get; set; } = string.Empty;
    public int Id { get; set; }
    
    public int AverageRating { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    public List<DtoGenre>? Genres { get; set; }
    public List<DtoPerson>? People { get; set; }
}