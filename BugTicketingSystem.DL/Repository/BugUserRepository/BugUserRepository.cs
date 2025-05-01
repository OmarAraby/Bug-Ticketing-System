using BugTicketingSystem.DL.Context;
using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Repository.BugUserRepository
{
    public class BugUserRepository : IBugUserRepository
    {
        private readonly BugTicketingSystemDbContext _context;

        public BugUserRepository(BugTicketingSystemDbContext context)
        {
            _context = context;
        }

        public async Task AssignUserToBug(Guid bugId, Guid userId)
        {
            var bugUser = new BugUser { BugId = bugId, UserId = userId };
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
    }
}
