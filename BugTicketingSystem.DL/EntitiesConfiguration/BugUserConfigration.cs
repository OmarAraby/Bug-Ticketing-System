using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.EntitiesConfiguration
{
    public class BugUserConfiguration : IEntityTypeConfiguration<BugUser>
    {
        public void Configure(EntityTypeBuilder<BugUser> builder)
        {
            // Composite primary key
            builder.HasKey(bu => new { bu.BugId, bu.UserId });

            builder.Property(bu => bu.AssignedDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Relation with Bug
            builder.HasOne(bu => bu.Bug)
                    .WithMany(b => b.Assignees)
                    .HasForeignKey(bu => bu.BugId)
                    .OnDelete(DeleteBehavior.Cascade);

            // Relation with User
            builder.HasOne(bu => bu.User)
                   .WithMany(u => u.AssignedBugs)
                   .HasForeignKey(bu => bu.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            }
    }

}
