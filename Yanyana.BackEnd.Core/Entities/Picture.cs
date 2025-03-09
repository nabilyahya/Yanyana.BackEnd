using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanyana.BackEnd.Core.Entities
{
    public class Picture
    {
        public int PictureId { get; set; }
        public string PicturePath { get; set; }
        public string Caption { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Relationships
        public int PlaceId { get; set; }
        public Place Place { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
