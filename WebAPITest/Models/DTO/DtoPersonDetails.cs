using WebAPITest.Models.TMDB;

namespace WebAPITest.Models.DTO;

public class DtoPersonDetails
{
    public string Image { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public List<DtoMovie> MovieCredits { get; set; } = new();
}