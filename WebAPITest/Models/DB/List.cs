using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class List
    {
        public List()
        {
            Listfilms = new HashSet<Listfilm>();
            Memberlikelists = new HashSet<Memberlikelist>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }
        public virtual ICollection<Listfilm> Listfilms { get; set; }
        public virtual ICollection<Memberlikelist> Memberlikelists { get; set; }
    }
}
