using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;

namespace WebAPITest.Factories;

public class GenreFactory
{
    private readonly FilmplattformContext _db;

    public GenreFactory(IConfiguration configuration, FilmplattformContext db)
    {
        _db = db;
        configuration.GetValue<string>("TmdbApiKey");
    }

    public Genre CreateGenre(int id, string name)
    {
        return new Genre
        {
            Id = id,
            Name = name
        };   
    }
}