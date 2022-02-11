using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Following
    {
        public int FollowingFollowerMemberId { get; set; }
        public int FollowingFollowingMemberId1 { get; set; }

        public virtual Member FollowingFollowerMember { get; set; }
        public virtual Member FollowingFollowingMemberId1Navigation { get; set; }
    }
}
