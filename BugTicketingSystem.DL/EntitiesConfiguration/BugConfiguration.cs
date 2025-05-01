using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BugTicketingSystem.DL.EntitiesConfiguration
{
   public class BugConfiguration : IEntityTypeConfiguration<Bug>
    {
        public void Configure(EntityTypeBuilder<Bug> builder)
        {
            builder.HasKey(b => b.BugId);

            builder.Property(b => b.BugName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(b => b.Description)
                .HasMaxLength(2000);

            builder.Property(b => b.Status)
                .IsRequired()
                .HasDefaultValue(BugStatus.New);

            builder.Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Relation with Project
            builder.HasOne(b => b.Project)
                .WithMany(p => p.Bugs)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
