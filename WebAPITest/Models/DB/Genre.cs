using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class Genre
    {
        public Genre()
        {
            Filmgenres = new HashSet<Filmgenre>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Filmgenre> Filmgenres { get; set; }
    }
}
