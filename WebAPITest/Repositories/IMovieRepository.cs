using WebAPITest.Models.DB;

namespace WebAPITest.Repositories;

public interface IMovieRepository
{
    Task<Film> GetMovie(int movieId);
    Task<Film> AddMovie(Film movie);
    Task<Film> UpdateMovie(Film movie);
}