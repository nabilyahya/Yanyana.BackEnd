using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Yanyana.BackEnd.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        // Relationships
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<UserPicture>  UserPictures { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rate> Rates { get; set; }
        public ICollection<Place> FavoritePlaces { get; set; }
    }
}
