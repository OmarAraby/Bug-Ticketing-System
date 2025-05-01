

namespace BugTicketingSystem.DL.Models
{
   public class BugUser
    {
        public Guid BugId { get; set; }
        public Bug Bug { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public RoleType Role { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;

    }
}
