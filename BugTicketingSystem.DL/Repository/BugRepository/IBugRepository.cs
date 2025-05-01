using BugTicketingSystem.DL.Models;


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
