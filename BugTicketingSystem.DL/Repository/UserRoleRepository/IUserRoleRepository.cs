using BugTicketingSystem.DL.Models;

namespace BugTicketingSystem.DL.Repository.UserRoleRepository
{
    public interface IUserRoleRepository
    {
        Task AddRoleToUser(Guid userId, RoleType role);
        Task RemoveRoleFromUser(Guid userId, RoleType role);
        Task<bool> UserHasRole(Guid userId, RoleType role);
        Task<IEnumerable<RoleType>> GetUserRoles(Guid userId);
    }
}
