using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Filmperson
    {
        public int FilmPersonPersonId { get; set; }
        public int FilmPersonFilmId { get; set; }
        public int FilmPersonPersonTypeId { get; set; }

        public virtual Film FilmPersonFilm { get; set; }
        public virtual Person FilmPersonPerson { get; set; }
        public virtual Persontype FilmPersonPersonType { get; set; }
    }
}
