using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yanyana.BackEnd.Core.Entities;

namespace Yanyana.BackEnd.Business.Dtos
{
    public class AuthResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }
    }
}
