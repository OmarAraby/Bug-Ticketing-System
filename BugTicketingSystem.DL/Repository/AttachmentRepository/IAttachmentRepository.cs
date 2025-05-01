using BugTicketingSystem.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Repository.AttachmentRepository
{
    public interface IAttachmentRepository
    {
        Task<Attachment> GetByIdAsync(Guid id);
        Task<IEnumerable<Attachment>> GetByBugIdAsync(Guid bugId);
        Task AddAsync(Attachment attachment);
        Task DeleteAsync(Attachment attachment);
    }
}
