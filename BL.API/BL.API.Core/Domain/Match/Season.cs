using System;

namespace BL.API.Core.Domain.Match
{
    public class Season : BaseEntity
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public bool OnGoing { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Finished { get; set; }
    }
}
