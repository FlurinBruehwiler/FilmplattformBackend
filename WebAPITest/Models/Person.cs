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

        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        public virtual ICollection<Filmperson> Filmpeople { get; set; }
    }
}
