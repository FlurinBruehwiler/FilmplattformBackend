using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class Member
    {
        public Member()
        {
            Filmmembers = new HashSet<Filmmember>();
            FollowingFollowers = new HashSet<Following>();
            FollowingFollowingNavigations = new HashSet<Following>();
            Lists = new HashSet<List>();
            Memberlikelists = new HashSet<Memberlikelist>();
            Watchevents = new HashSet<Watchevent>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public virtual ICollection<Filmmember> Filmmembers { get; set; }
        public virtual ICollection<Following> FollowingFollowers { get; set; }
        public virtual ICollection<Following> FollowingFollowingNavigations { get; set; }
        public virtual ICollection<List> Lists { get; set; }
        public virtual ICollection<Memberlikelist> Memberlikelists { get; set; }
        public virtual ICollection<Watchevent> Watchevents { get; set; }
    }
}
