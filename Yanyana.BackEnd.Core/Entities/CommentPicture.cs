using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yanyana.BackEnd.Core.Entities
{
    public class CommentPicture
    {
        public int CommentPictureId { get; set; }
        public string PicturePath { get; set; }
        public string Caption { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Relationships
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
