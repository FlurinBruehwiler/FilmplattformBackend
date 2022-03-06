using WebAPITest.Models.DB;

namespace WebAPITest.Services.MovieService;

public interface IMovieService
{
    Task<bool> PatchLike(int movieId, bool like);
    Task<bool> PatchWatchlist(int movieId, bool watchlist);
    Film? GetMovieById(int id);
    string? GetDirector(int id);
}