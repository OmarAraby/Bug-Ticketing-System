using BugTicketingSystem.DL.Repository.AttachmentRepository;
using BugTicketingSystem.DL.Repository.BugRepository;
using BugTicketingSystem.DL.Repository.BugUserRepository;
using BugTicketingSystem.DL.Repository.ProjectRepository;
using BugTicketingSystem.DL.Repository.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IBugRepository BugRepository { get; }
        IBugUserRepository BugUserRepository { get; }
        IAttachmentRepository AttachmentRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
