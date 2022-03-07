using Microsoft.EntityFrameworkCore;
using WebAPITest.Factories;
using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;

namespace WebAPITest.Services;

public class GenreService
{
    private readonly FilmplattformContext _db;
    private readonly GenreFactory _genreFactory;

    public GenreService(FilmplattformContext db, GenreFactory genreFactory)
    {
        _db = db;
        _genreFactory = genreFactory;
    }
    
    public List<Genre> GetGenresForMovie(TMDBMovieDetails tmdbMovie)
    {
        List<Genre> genres = new();

        var tmdbGenres = tmdbMovie.Genres;
        
        if (tmdbGenres == null)
            return genres;
        
        foreach (var tmdbGenre in tmdbGenres)
        {
            genres.Add(GetGenre(tmdbGenre));
        }

        return genres;
    }
    
    private Genre GetGenre(TMDBGenre tmdbGenre)
    {
        if (GenreExists(tmdbGenre.Id))
            return _db.Genres.First(x => x.Id == tmdbGenre.Id);

        var genre = _genreFactory.CreateGenre(tmdbGenre.Id, tmdbGenre.Name);
        
        return genre;
    }
    
    private bool GenreExists(int id)
    {
        return _db.Genres.AsNoTracking().Any(x => x.Id == id);
    }
}