using WebAPITest.Models.DB;

namespace WebAPITest.Models.DTO;

public class DtoPerson
{
    public DtoPerson(Filmperson filmperson)
    {
        Id = filmperson.Person.Id;
        Name = filmperson.Person.Name;
        Department = filmperson.PersonType.Name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
}