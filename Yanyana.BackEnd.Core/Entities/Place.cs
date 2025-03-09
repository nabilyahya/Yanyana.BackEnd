using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Yanyana.BackEnd.Core.Entities
{
    public class Place
    {
        public int PlaceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        // Relationships
        public int LocationId { get; set; }
        public Location Location { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rate> Rates { get; set; }
        public ICollection<Picture> Pictures { get; set; }
        public ICollection<User> FavoriteUsers { get; set; }
        public ICollection<PlaceCategory> PlaceCategories { get; set; }
        public GoogleLocation GoogleLocation { get; set; }
    }
}
