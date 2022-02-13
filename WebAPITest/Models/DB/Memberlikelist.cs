#nullable disable

namespace WebAPITest.Models.DB
{
    public partial class Memberlikelist
    {
        public int MemberId { get; set; }
        public int ListId { get; set; }

        public virtual List List { get; set; }
        public virtual Member Member { get; set; }
    }
}
