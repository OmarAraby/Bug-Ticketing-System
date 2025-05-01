using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BugTicketingSystem.DL.Context;
using BugTicketingSystem.DL.Models;
using BugTicketingSystem.DL.Repository;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using BugTicketingSystem.DL.Models;
using BugTicketingSystem.DL.Repository.AttachmentRepository;
using BugTicketingSystem.DL.Repository.BugRepository;
using BugTicketingSystem.DL.Repository.BugUserRepository;
using BugTicketingSystem.DL.Repository.ProjectRepository;
using BugTicketingSystem.DL.Repository.UserRepository;
using BugTicketingSystem.DL.UnitOfWork;
using Org.BouncyCastle.Crypto.Generators;

namespace BugTicketingSystem.DAL
{
    public static class DataAccessExtensions
    {
        public static void AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("db");

            services.AddDbContext<BugTicketingSystemDbContext>(options => options
                .UseSqlServer(connectionString)
                .UseSeeding((context, _) =>
                {
                    if (context.Set<User>().Any())
                        return;

               
                    var roles = new List<RoleType>
                    {
                        RoleType.Manager,
                        RoleType.Developer,
                        RoleType.Tester
                    };

                 
                    var users = new List<User>
                    {
                        new User
                        {
                            UserId = Guid.NewGuid(),
                            UserName = "admin",
                            Email = "admin@bugsystem.com",
                            PasswordHash = HashPassword("Admin@123"),
                            UserRoles = new List<UserRole>
                            {
                                new UserRole { Role = RoleType.Manager }
                            }
                        },
                        new User
                        {
                            UserId = Guid.NewGuid(),
                            UserName = "dev1",
                            Email = "dev1@bugsystem.com",
                            PasswordHash = HashPassword("Dev@123"),
                            UserRoles = new List<UserRole>
                            {
                                new UserRole { Role = RoleType.Developer }
                            }
                        }
                    };

                 
                    var projects = new List<Project>
                    {
                        new() { ProjectId = Guid.NewGuid(), ProjectName = "Website Redesign", Description = "Redesign of company website" },
                        new() { ProjectId = Guid.NewGuid(), ProjectName = "Mobile App", Description = "New mobile application development" }
                    };

                    context.AddRange(users);
                    context.AddRange(projects);
                    context.SaveChanges();
                })
                .UseAsyncSeeding(async (context, _, _) =>
                {
                    if (await context.Set<Bug>().AnyAsync())
                        return;

                    var projectId = await context.Set<Project>().Select(p => p.ProjectId).FirstAsync();
                    var userId = await context.Set<User>().Select(u => u.UserId).FirstAsync();

                    var bugs = new List<Bug>
                    {
                        new()
                        {
                            BugId = Guid.NewGuid(),
                            BugName = "Login Page Not Responsive",
                            Description = "Login page breaks on mobile devices",
                            Status = BugStatus.New,
                            CreatedAt = DateTime.UtcNow,
                            ProjectId = projectId
                        }
                    };

                    var bugUsers = new List<BugUser>
                    {
                        new() { BugId = bugs[0].BugId, UserId = userId, AssignedDate = DateTime.UtcNow }
                    };

                    await context.AddRangeAsync(bugs);
                    await context.AddRangeAsync(bugUsers);
                    await context.SaveChangesAsync();
                }));

         
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IBugRepository, BugRepository>();
            services.AddScoped<IBugUserRepository, BugUserRepository>();
            services.AddScoped<IAttachmentRepository, AttachmentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}