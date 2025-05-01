

namespace BugTicketingSystem.DL.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public ICollection<BugUser> AssignedBugs { get; set; } = new HashSet<BugUser>();

    }
}
