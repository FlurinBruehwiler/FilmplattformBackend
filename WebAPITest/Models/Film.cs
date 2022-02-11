using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Film
    {
        public Film()
        {
            Filmgenres = new HashSet<Filmgenre>();
            Filmmembers = new HashSet<Filmmember>();
            Filmpeople = new HashSet<Filmperson>();
            Watchevents = new HashSet<Watchevent>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public virtual ICollection<Filmgenre> Filmgenres { get; set; }
        public virtual ICollection<Filmmember> Filmmembers { get; set; }
        public virtual ICollection<Filmperson> Filmpeople { get; set; }
        public virtual ICollection<Watchevent> Watchevents { get; set; }
    }
}
