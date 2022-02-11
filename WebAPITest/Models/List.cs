using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class List
    {
        public List()
        {
            Memberlikelists = new HashSet<Memberlikelist>();
        }

        public int ListId { get; set; }
        public string ListName { get; set; }
        public int ListCreatorMemberId { get; set; }

        public virtual Member ListCreatorMember { get; set; }
        public virtual ICollection<Memberlikelist> Memberlikelists { get; set; }
    }
}
