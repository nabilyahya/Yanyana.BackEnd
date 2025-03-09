using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanyana.BackEnd.Core.Entities
{
    public class UserPicture
    {
        public int UserPictureId { get; set; }
        public string PicturePath { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Relationships
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
