

namespace BugTicketingSystem.BL.Dtos.BugDtos
{
    public class BugListDto
    {
        public Guid BugId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string ProjectName { get; set; }
    }
}
