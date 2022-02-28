using WebAPITest.Models.DB;

namespace WebAPITest.Repositories;

public class MovieRepository : IMovieRepository
{
    public Task<Film> GetMovie(int movieId)
    {
        throw new NotImplementedException();
    }

    public Task<Film> AddMovie(Film movie)
    {
        throw new NotImplementedException();
    }

    public Task<Film> UpdateMovie(Film movie)
    {
        throw new NotImplementedException();
    }
}