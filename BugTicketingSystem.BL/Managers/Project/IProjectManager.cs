// IProjectManager.cs
using BugTicketingSystem.BL.Dtos.ProjectDtos;
using BugTicketingSystem.BL.Utils.Error;

namespace BugTicketingSystem.BL.Managers.Project
{
    public interface IProjectManager
    {
        Task<APIResult<ProjectDetailsDto>> Create(ProjectCreateDto dto);
        Task<APIResult<IEnumerable<ProjectDetailsDto>>> GetAll();
        Task<APIResult<ProjectDetailsDto>> GetDetails(Guid id);
    }
}