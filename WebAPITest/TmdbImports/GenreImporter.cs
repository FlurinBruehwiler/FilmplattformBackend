using Microsoft.EntityFrameworkCore;
using WebAPITest.Models.DB;
using WebAPITest.Models.TMDB;

namespace WebAPITest.TmdbImports;

public class GenreImporter
{
    private readonly FilmplattformContext _db;

    public GenreImporter(IConfiguration configuration, FilmplattformContext db)
    {
        _db = db;
        configuration.GetValue<string>("TmdbApiKey");
    }
    
    public List<Genre> GetGenresForMovie(TMDBMovieDetails? tmdbMovie)
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

        var genre = new Genre
        {
            Id = tmdbGenre.Id,
            Name = tmdbGenre.Name
        };
        
        return genre;
    }
    
    private bool GenreExists(int id)
    {
        return _db.Genres.AsNoTracking().Any(x => x.Id == id);
    }
}