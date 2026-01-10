using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DateTrensferObjects
{
    public class UserRolesDto
    {
        public Guid UserId { get; set; }
        public List<string> RoleNames { get; set; } = new();
    }
}
