using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class Listfilm
    {
        public int FilmId { get; set; }
        public int ListId { get; set; }

        public virtual Film Film { get; set; }
        public virtual List List { get; set; }
    }
}
