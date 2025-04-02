using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanyana.BackEnd.Core.Entities
{
    public class Rate
    {
        public int RateId { get; set; }
        public int Value { get; set; } // Typically 1-5 stars
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User User { get; set; }

        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}

