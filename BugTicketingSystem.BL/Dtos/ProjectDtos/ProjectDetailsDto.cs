using BugTicketingSystem.BL.Dtos.BugDtos;
namespace BugTicketingSystem.BL.Dtos.ProjectDtos
{
    public class ProjectDetailsDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int BugsCount { get; set; }
        public ICollection<BugDetailsDto> Bugs { get; set; }

    }
}