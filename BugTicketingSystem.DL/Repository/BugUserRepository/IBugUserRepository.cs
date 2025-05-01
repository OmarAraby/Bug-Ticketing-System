using BugTicketingSystem.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Repository.BugUserRepository
{
    public interface IBugUserRepository
    {
        Task AssignUserToBug(Guid bugId, Guid userId, RoleType role); 
        Task RemoveUserFromBug(Guid bugId, Guid userId);
        Task<bool> IsUserAssignedToBug(Guid bugId, Guid userId);
        Task<IEnumerable<BugUser>> GetAssigneesForBugAsync(Guid bugId); 
    }
}
