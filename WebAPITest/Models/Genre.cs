using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Filmgenres = new HashSet<Filmgenre>();
        }

        public int GenreId { get; set; }
        public string GenreName { get; set; }

        public virtual ICollection<Filmgenre> Filmgenres { get; set; }
    }
}
