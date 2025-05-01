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
            => await _context.Bugs.FindAsync(id);

        public async Task<IEnumerable<Bug>> GetAllAsync()
            => await _context.Bugs.ToListAsync();

        public async Task<IEnumerable<Bug>> GetByProjectIdAsync(Guid projectId)
            => await _context.Bugs.Where(b => b.ProjectId == projectId).ToListAsync();

        public async Task AddAsync(Bug bug)
            => await _context.Bugs.AddAsync(bug);

        public async Task UpdateAsync(Bug bug)
            => _context.Bugs.Update(bug);
    }
}
