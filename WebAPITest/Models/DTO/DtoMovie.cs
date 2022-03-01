using WebAPITest.Models.DB;

namespace WebAPITest.Models.DTO;

public class DtoMovie
{
    public DtoMovie(Film film)
    {
        Id = film.Id;
        Title = film.Title;
        ShortDescription = film.ShortDescription;
        LongDescription = film.LongDescription;
        PosterURL = film.PosterUrl;
        BackdropURL = film.BackdropUrl;
        ReleaseDate = film.ReleaseDate;
        Genres = film.Filmgenres.Select(x => new DtoGenre(x.Genre)).ToList();
        People = film.Filmpeople.Select(x => new DtoPerson(x)).ToList();
    }

    public string PosterURL { get; set; }
    public string BackdropURL { get; set; }
    public int Id { get; set; }
    
    public float AverageRating { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    public List<DtoGenre>? Genres { get; set; }
    public List<DtoPerson>? People { get; set; }
}