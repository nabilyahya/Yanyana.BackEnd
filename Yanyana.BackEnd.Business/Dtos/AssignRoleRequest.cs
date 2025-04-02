using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yanyana.BackEnd.Core.Entities;
using static Yanyana.BackEnd.Core.Enums.Enums;

namespace Yanyana.BackEnd.Business.Dtos
{
    public class AssignRoleRequest
    {
        public RoleEnum Role { get; set; }
    }
}
