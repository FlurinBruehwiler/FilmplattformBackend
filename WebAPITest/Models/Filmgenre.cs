using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Filmgenre
    {
        public int FilmGenreGenreId { get; set; }
        public int FilmGenreFilmId { get; set; }

        public virtual Film FilmGenreFilm { get; set; }
        public virtual Genre FilmGenreGenre { get; set; }
    }
}
