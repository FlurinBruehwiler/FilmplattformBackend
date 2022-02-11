using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Filmmember
    {
        public int FilmMemberMemberId { get; set; }
        public int FilmMemberFilmId { get; set; }
        public byte? FilmMemberLike { get; set; }
        public DateTime? FilmMemberLikeDate { get; set; }
        public byte? FilmMemberWatchlist { get; set; }
        public DateTime? FilmMemberWatchlistDate { get; set; }

        public virtual Film FilmMemberFilm { get; set; }
        public virtual Member FilmMemberMember { get; set; }
    }
}
