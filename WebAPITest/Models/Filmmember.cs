using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Filmmember
    {
        public int MemberId { get; set; }
        public int FilmId { get; set; }
        public byte? Like { get; set; }
        public DateTime? LikeDate { get; set; }
        public byte? Watchlist { get; set; }
        public DateTime? WatchlistDate { get; set; }

        public virtual Film Film { get; set; }
        public virtual Member Member { get; set; }
    }
}
