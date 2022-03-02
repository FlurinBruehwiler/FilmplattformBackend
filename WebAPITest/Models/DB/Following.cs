using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class Following
    {
        public int FollowerId { get; set; }
        public int FollowingId { get; set; }

        public virtual Member Follower { get; set; }
        public virtual Member FollowingNavigation { get; set; }
    }
}
