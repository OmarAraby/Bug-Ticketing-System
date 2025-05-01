using BugTicketingSystem.DL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace BugTicketingSystem.DL.EntitiesConfiguration
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.HasKey(a => a.AttachmentId);

            builder.Property(a => a.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(a => a.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.UploadedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Relation with Bug
            builder.HasOne(a => a.Bug)
                .WithMany(b => b.Attachments)
                .HasForeignKey(a => a.BugId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
