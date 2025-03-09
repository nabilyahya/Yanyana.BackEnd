using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanyana.BackEnd.Core.Entities
{
    public class District
    {
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        // Relationships
        public int CityId { get; set; }
        public City City { get; set; }

        public ICollection<Street> Streets { get; set; }
        public ICollection<Location> Locations { get; set; }
    }
}
