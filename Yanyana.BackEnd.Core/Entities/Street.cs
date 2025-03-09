using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanyana.BackEnd.Core.Entities
{
    public class Street
    {
        public int StreetId { get; set; }
        public string StreetName { get; set; }
        public string PostalCode { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        // Relationships
        public int DistrictId { get; set; }
        public District District { get; set; }

        public ICollection<Location> Locations { get; set; }
    }
}
