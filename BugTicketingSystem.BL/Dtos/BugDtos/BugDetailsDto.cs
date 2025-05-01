using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Dtos.BugDtos
{
    public class BugDetailsDto
    {
        public Guid BugId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public ProjectBasicDto Project { get; set; }
        public List<UserBasicDto> Assignees { get; set; }

    }
    // Supporting DTOs
    public class ProjectBasicDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
    }

    public class UserBasicDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; } 
    }
}
