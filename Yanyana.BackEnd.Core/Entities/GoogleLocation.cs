using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanyana.BackEnd.Core.Entities
{
    public class GoogleLocation
    {
        public int GoogleLocationId { get; set; }
        public string GooglePlaceId { get; set; } // Unique identifier from Google
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public string FormattedAddress { get; set; }
        public string GoogleData { get; set; } // JSON data from Google API
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        // Relationships
        public int PlaceId { get; set; }
        public Place Place { get; set; }
    }
}
