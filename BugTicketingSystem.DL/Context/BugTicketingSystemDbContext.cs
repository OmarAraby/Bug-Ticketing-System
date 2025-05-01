using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Context
{
    public class BugTicketingSystemDbContext : DbContext
    {

        // DbSet properties for your entities
       public DbSet<Bug> Bugs => Set<Bug>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Attachment> Attachments => Set<Attachment>();
        public DbSet<BugUser> BugUsers => Set<BugUser>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserRole> Roles => Set<UserRole>();


        public BugTicketingSystemDbContext(DbContextOptions<BugTicketingSystemDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the model using Fluent API if needed
            // For example, you can set up relationships, constraints, etc.
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BugTicketingSystemDbContext).Assembly);
        }
    }
}
