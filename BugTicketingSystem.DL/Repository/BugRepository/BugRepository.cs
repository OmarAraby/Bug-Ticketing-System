using BugTicketingSystem.DL.Context;
using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Repository.BugRepository
{
    public class BugRepository : IBugRepository
    {
        private readonly BugTicketingSystemDbContext _context;

        public BugRepository(BugTicketingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Bug> GetByIdAsync(Guid id)
        {
            return await _context.Bugs
               .Include(b => b.Project) // Include Project
                .Include(b => b.Assignees) // Include Assignees
                    .ThenInclude(bu => bu.User) // Include User for each BugUser
                .FirstOrDefaultAsync(b => b.BugId == id);
        }

        public async Task<IEnumerable<Bug>> GetAllAsync()
        {
            return await _context.Bugs
                .Include(b => b.Project) // Include the Project navigation property
                .ToListAsync();
        }
        public async Task<IEnumerable<Bug>> GetByProjectIdAsync(Guid projectId)
            => await _context.Bugs.Where(b => b.ProjectId == projectId).ToListAsync();

        public async Task AddAsync(Bug bug)
            => await _context.Bugs.AddAsync(bug);

        public async Task UpdateAsync(Bug bug)
            => _context.Bugs.Update(bug);
    }
}
