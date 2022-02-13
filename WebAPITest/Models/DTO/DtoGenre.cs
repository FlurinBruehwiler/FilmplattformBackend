using WebAPITest.Models.DB;

namespace WebAPITest.Models.DTO;

public class DtoGenre
{
    public DtoGenre(Genre genre)
    {
        Id = genre.Id;
        Name = genre.Name;
    }
    
    public int Id { get; set; }
    public string? Name { get; set; }
}