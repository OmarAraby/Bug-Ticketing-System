using BugTicketingSystem.DL.Context;
using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;

namespace BugTicketingSystem.DL.Repository.UserRoleRepository
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly BugTicketingSystemDbContext _context;

        public UserRoleRepository(BugTicketingSystemDbContext context)
        {
            _context = context;
        }

        public async Task AddRoleToUser(Guid userId, RoleType role)
        {
            var userRole = new UserRole { UserId = userId, Role = role };
            await _context.Roles.AddAsync(userRole);
        }

        public async Task RemoveRoleFromUser(Guid userId, RoleType role)
        {
            var userRole = await _context.Roles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.Role == role);

            if (userRole != null)
                _context.Roles.Remove(userRole);
        }

        public async Task<bool> UserHasRole(Guid userId, RoleType role)
            => await _context.Roles
                .AnyAsync(ur => ur.UserId == userId && ur.Role == role);

        public async Task<IEnumerable<RoleType>> GetUserRoles(Guid userId)
            => await _context.Roles
                .Where(ur => ur.UserId == userId)
                .Select(ur => ur.Role)
                .ToListAsync();
    }
}
