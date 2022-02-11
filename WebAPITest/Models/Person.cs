using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Person
    {
        public Person()
        {
            Filmpeople = new HashSet<Filmperson>();
        }

        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public string PersonBio { get; set; }

        public virtual ICollection<Filmperson> Filmpeople { get; set; }
    }
}
