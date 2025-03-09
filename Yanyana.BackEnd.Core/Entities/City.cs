using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanyana.BackEnd.Core.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        // Relationships
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public ICollection<Location> Locations { get; set; }
        public ICollection<Street> Streets { get; set; }
    }
}
