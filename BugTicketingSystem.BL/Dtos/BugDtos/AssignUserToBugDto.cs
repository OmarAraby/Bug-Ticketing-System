using BugTicketingSystem.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Dtos.BugDtos
{
    public class AssignUserToBugDto
    {
        public Guid UserId { get; set; }
        public RoleType Role { get; set; }
    }
}
