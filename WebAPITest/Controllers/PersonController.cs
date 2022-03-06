using Microsoft.AspNetCore.Mvc;
using WebAPITest.Models.DTO;
using WebAPITest.Models.TMDB;
using WebAPITest.Services.PersonService;

namespace WebAPITest.Controllers;

[Controller]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IPersonService _personService;
    private readonly string _apiKey;
    
    public PersonController(IConfiguration configuration, IHttpClientFactory clientFactory,
        IPersonService personService)
    {
        _clientFactory = clientFactory;
        _personService = personService;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
    }

    [HttpGet("{personId}")]
    public async Task<ActionResult<DtoPersonDetails>> GetActor(int personId)
    {
        var person = _personService.GetPersonById(personId);

        if (person is null)
            return NotFound($"No Person with the Id {person} found");

        var url = $"person/{personId}/movie_credits?api_key={_apiKey}";

        TmdbPersonCredits? credits;
        var client = _clientFactory.CreateClient("tmdb");

        try
        {
            credits = await client.GetFromJsonAsync<TmdbPersonCredits>(url);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

        if (credits == null)
            return StatusCode(500);

        return Ok(new DtoPersonDetails
        {
            Bio = person.Bio,
            Name = person.Name,
            MovieCredits = credits.Cast.Concat(credits.Crew)
                .Select(x => new DtoMovie
                {
                    Id = x.MovieId, 
                    Title = x.Title, 
                    PosterPath = x.PosterPath
                }).ToList()
        });
    }
}