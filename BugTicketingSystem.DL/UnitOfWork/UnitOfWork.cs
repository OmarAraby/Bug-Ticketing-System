using BugTicketingSystem.DL.Context;
using BugTicketingSystem.DL.Repository.AttachmentRepository;
using BugTicketingSystem.DL.Repository.BugRepository;
using BugTicketingSystem.DL.Repository.BugUserRepository;
using BugTicketingSystem.DL.Repository.ProjectRepository;
using BugTicketingSystem.DL.Repository.UserRepository;


namespace BugTicketingSystem.DL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BugTicketingSystemDbContext _context;

        public IUserRepository UserRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public IBugRepository BugRepository { get; }
        public IBugUserRepository BugUserRepository { get; }
        public IAttachmentRepository AttachmentRepository { get; }


        public UnitOfWork(BugTicketingSystemDbContext context , IUserRepository userRepository,IProjectRepository projectRepository, IBugRepository bugRepository, IBugUserRepository bugUserRepository , IAttachmentRepository attachmentRepository  )
        {
            _context = context;
            UserRepository = userRepository;
            ProjectRepository = projectRepository;
            BugRepository = bugRepository;
            BugUserRepository = bugUserRepository;
            AttachmentRepository = attachmentRepository;

        }
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
