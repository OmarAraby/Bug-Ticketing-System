using BugTicketingSystem.DL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Repository.BugRepository
{
    public interface IBugRepository
    {
        Task<Bug> GetByIdAsync(Guid id);
        Task<IEnumerable<Bug>> GetAllAsync();
        Task<IEnumerable<Bug>> GetByProjectIdAsync(Guid projectId);
        Task AddAsync(Bug bug);
        Task UpdateAsync(Bug bug);
    }
}
