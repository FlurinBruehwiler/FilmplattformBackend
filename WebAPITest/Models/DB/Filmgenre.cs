#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class Filmgenre
    {
        public int GenreId { get; set; }
        public int FilmId { get; set; }

        public virtual Film Film { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
