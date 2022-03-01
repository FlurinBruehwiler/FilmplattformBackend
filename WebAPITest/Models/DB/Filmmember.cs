using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebAPITest.Models.DB
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
        
        [NotMapped]
        public bool LikeBool
        {
            get => Like > 0;
            set => this.Like = (byte)(value ? 1 : 0);
        }
        
        [NotMapped]
        public bool WatchlistBool
        {
            get => Watchlist > 0;
            set => this.Watchlist = (byte)(value ? 1 : 0);
        }
    }
}
