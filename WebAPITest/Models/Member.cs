using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Member
    {
        public Member()
        {
            Filmmembers = new HashSet<Filmmember>();
            Lists = new HashSet<List>();
            Memberlikelists = new HashSet<Memberlikelist>();
            Watchevents = new HashSet<Watchevent>();
        }

        public int MemberId { get; set; }
        public string MemberUsername { get; set; }
        public string MemberName { get; set; }
        public string MemberVorname { get; set; }
        public string MemberEmail { get; set; }
        public string MemberBio { get; set; }

        public virtual ICollection<Filmmember> Filmmembers { get; set; }
        public virtual ICollection<List> Lists { get; set; }
        public virtual ICollection<Memberlikelist> Memberlikelists { get; set; }
        public virtual ICollection<Watchevent> Watchevents { get; set; }
    }
}
