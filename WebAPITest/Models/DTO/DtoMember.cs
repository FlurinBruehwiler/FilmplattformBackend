using Microsoft.Extensions.Primitives;

namespace WebAPITest.Models.DTO;

public class DtoMember
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string ProfilePicturePath { get; set; } = string.Empty;
}