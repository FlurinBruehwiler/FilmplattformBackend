using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Memberlikelist
    {
        public int MemberLikeListMemberId { get; set; }
        public int MemberLikeListListId { get; set; }

        public virtual List MemberLikeListList { get; set; }
        public virtual Member MemberLikeListMember { get; set; }
    }
}
