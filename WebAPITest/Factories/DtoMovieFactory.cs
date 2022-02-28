using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Services.UserService;

namespace WebAPITest.Factories;

public class DtoMovieFactory
{
    private readonly FilmplattformContext _db;
    private readonly IUserService _userService;

    public DtoMovieFactory(FilmplattformContext db, IUserService userService)
    {
        _db = db;
        _userService = userService;
    }
    
    public DtoMovie? GetDtoMovie(int movieId)
    {
        var film = _db.Films.Where(movie => movie.Id == movieId)
            .Include(movie => movie.Filmgenres)
            .ThenInclude(movieGenre => movieGenre.Genre)
            .Include(movie => movie.Filmpeople)
            .ThenInclude(t => t.Person)
            .Include(movie => movie.Filmpeople)
            .ThenInclude(t => t.PersonType)
            .FirstOrDefault();
        
        Console.WriteLine($"The User Id is {_userService.GetId()}");

        if (film == null)
        {
            return null;
        }
        
        return new DtoMovie(film);
    }
}