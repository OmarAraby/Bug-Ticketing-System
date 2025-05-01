using BugTicketingSystem.BL.Dtos.BugDtos;
using BugTicketingSystem.BL.Utils.Error;
using BugTicketingSystem.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Managers.Bug
{
    public interface IBugManager
    {
        Task<APIResult<BugDetailsDto>> Create(BugCreateDto dto);
        Task<APIResult<IEnumerable<BugListDto>>> GetAll();
        Task<APIResult<BugDetailsDto>> GetDetails(Guid id);
        Task<APIResult<IEnumerable<BugListDto>>> GetByProjectId(Guid projectId);

        // User Bug Assignment Methods
        Task<APIResult> AssignUserToBug(Guid bugId, AssignUserToBugDto dto); 
        Task<APIResult> RemoveUserFromBug(Guid bugId, Guid userId);
    }
}

