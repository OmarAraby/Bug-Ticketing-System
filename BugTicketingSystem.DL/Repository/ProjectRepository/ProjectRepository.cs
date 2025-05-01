using BugTicketingSystem.DL.Context;
using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Repository.ProjectRepository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly BugTicketingSystemDbContext _context;

        public ProjectRepository(BugTicketingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Project> GetByIdAsync(Guid id)
            => await _context.Projects.FindAsync(id);

        public async Task<IEnumerable<Project>> GetAllAsync()
            => await _context.Projects.ToListAsync();

        public async Task AddAsync(Project project)
            => await _context.Projects.AddAsync(project);

        public async Task UpdateAsync(Project project)
            => _context.Projects.Update(project);
    }
}
