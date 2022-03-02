using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class Film
    {
        public Film()
        {
            Filmgenres = new HashSet<Filmgenre>();
            Filmmembers = new HashSet<Filmmember>();
            Filmpeople = new HashSet<Filmperson>();
            Listfilms = new HashSet<Listfilm>();
            Watchevents = new HashSet<Watchevent>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public string PosterUrl { get; set; }
        public string BackdropUrl { get; set; }

        public virtual ICollection<Filmgenre> Filmgenres { get; set; }
        public virtual ICollection<Filmmember> Filmmembers { get; set; }
        public virtual ICollection<Filmperson> Filmpeople { get; set; }
        public virtual ICollection<Listfilm> Listfilms { get; set; }
        public virtual ICollection<Watchevent> Watchevents { get; set; }
    }
}
