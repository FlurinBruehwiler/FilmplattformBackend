using WebAPITest.Models.DB;

namespace WebAPITest.Models.DTO;

public class DtoMoviePersonal : DtoMovie
{
    public DtoMoviePersonal(Film film) : base(film)
    {
        
    }
}