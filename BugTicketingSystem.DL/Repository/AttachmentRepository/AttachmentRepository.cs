using BugTicketingSystem.DL.Context;
using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Repository.AttachmentRepository
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly BugTicketingSystemDbContext _context;

        public AttachmentRepository(BugTicketingSystemDbContext context)
        {
            _context = context;
        }

        public async Task<Attachment> GetByIdAsync(Guid id)
            => await _context.Attachments.FindAsync(id);

        public async Task<IEnumerable<Attachment>> GetByBugIdAsync(Guid bugId)
            => await _context.Attachments
                .Where(a => a.BugId == bugId)
                .ToListAsync();

        public async Task AddAsync(Attachment attachment)
            => await _context.Attachments.AddAsync(attachment);

        public async Task DeleteAsync(Attachment attachment)
            => _context.Attachments.Remove(attachment);
    }
}
