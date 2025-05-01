

using System.Net.Mail;

namespace BugTicketingSystem.DL.Models
{
   public class Bug
    {
        public Guid BugId { get; set; }
        public string BugName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public BugStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }



        // Navigation properties
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public ICollection<BugUser> Assignees { get; set; } = new HashSet<BugUser>();
        public ICollection<Attachment> Attachments { get; set; } = new HashSet<Attachment>();
    }

    public enum BugStatus
    {
        New,
        InProgress,
        Resolved,
        Closed
    }
}
