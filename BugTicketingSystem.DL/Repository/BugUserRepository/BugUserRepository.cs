using BugTicketingSystem.DL.Context;
using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;


namespace BugTicketingSystem.DL.Repository.BugUserRepository
{
    public class BugUserRepository : IBugUserRepository
    {
        private readonly BugTicketingSystemDbContext _context;

        public BugUserRepository(BugTicketingSystemDbContext context)
        {
            _context = context;
        }

        public async Task AssignUserToBug(Guid bugId, Guid userId, RoleType role)
        {
            var bugUser = new BugUser
            {
                BugId = bugId,
                UserId = userId,
                Role = role,
                AssignedDate = DateTime.UtcNow
            };
            await _context.BugUsers.AddAsync(bugUser);
        }

        public async Task RemoveUserFromBug(Guid bugId, Guid userId)
        {
            var bugUser = await _context.BugUsers
                .FirstOrDefaultAsync(bu => bu.BugId == bugId && bu.UserId == userId);

            if (bugUser != null)
                _context.BugUsers.Remove(bugUser);
        }

        public async Task<bool> IsUserAssignedToBug(Guid bugId, Guid userId)
            => await _context.BugUsers
                .AnyAsync(bu => bu.BugId == bugId && bu.UserId == userId);

        public async Task<IEnumerable<BugUser>> GetAssigneesForBugAsync(Guid bugId)
            => await _context.BugUsers
                .Where(bu => bu.BugId == bugId)
                .Include(bu => bu.User) // Include User for mapping
                .ToListAsync();
    }
}
