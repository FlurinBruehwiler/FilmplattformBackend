using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models
{
    public partial class Watchevent
    {
        public int Id { get; set; }
        public int? Rating { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int MemberId { get; set; }
        public int FilmId { get; set; }

        public virtual Film Film { get; set; }
        public virtual Member Member { get; set; }
    }
}
