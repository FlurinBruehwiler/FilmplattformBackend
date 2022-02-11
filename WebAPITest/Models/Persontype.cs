using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
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
