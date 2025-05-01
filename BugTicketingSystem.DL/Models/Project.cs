

namespace BugTicketingSystem.DL.Models
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Bug> Bugs { get; set; } = new HashSet<Bug>();

    }
}
