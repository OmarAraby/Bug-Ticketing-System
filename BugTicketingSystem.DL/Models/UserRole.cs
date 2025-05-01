

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
