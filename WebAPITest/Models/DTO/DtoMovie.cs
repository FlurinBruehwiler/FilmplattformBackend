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
        Genres = film.Filmgenres.Select(x => new DtoGenre(x.Genre)).ToList();
        People = film.Filmpeople.Select(x => new DtoPerson(x)).ToList();
    }
    
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string? ShortDescription { get; set; }
    public string? LongDescription { get; set; }
    public List<DtoGenre>? Genres { get; set; }
    public List<DtoPerson>? People { get; set; }
}