using System;
using System.Collections.Generic;

#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class Filmperson
    {
        public int PersonId { get; set; }
        public int FilmId { get; set; }
        public int PersonTypeId { get; set; }

        public virtual Film Film { get; set; }
        public virtual Person Person { get; set; }
        public virtual Persontype PersonType { get; set; }
    }
}
