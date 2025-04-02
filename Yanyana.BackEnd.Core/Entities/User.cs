using System;
using System.Collections.Generic;
using System.Security.Claims;
using Yanyana.BackEnd.Core.Enums;

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
        public Role Role { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserPicture> UserPictures { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Rate> Rates { get; set; }
        public ICollection<Place> FavoritePlaces { get; set; }
        public List<Claim> Claims { get; set; }
    }
}