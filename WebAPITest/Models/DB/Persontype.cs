#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class Persontype
    {
        public Persontype()
        {
            Filmpeople = new HashSet<Filmperson>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Filmperson> Filmpeople { get; set; }
    }
}
