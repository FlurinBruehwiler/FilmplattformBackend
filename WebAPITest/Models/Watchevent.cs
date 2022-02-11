using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Watchevent
    {
        public int WatchEventId { get; set; }
        public int? WatchEventRating { get; set; }
        public string WatchEventText { get; set; }
        public DateTime WatchEventDate { get; set; }
        public int WatchEventMemberId { get; set; }
        public int WatchEventFilmId { get; set; }

        public virtual Film WatchEventFilm { get; set; }
        public virtual Member WatchEventMember { get; set; }
    }
}
