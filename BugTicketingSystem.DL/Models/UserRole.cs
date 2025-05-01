using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Models
{
   public class UserRole
    {
        public Guid UserId { get; set; }

        public User User { get; set; }

        public RoleType Role { get; set; }
    }

    public enum RoleType
    {
        Manager,
        Developer,
        Tester
    }
}
