using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.DL.Models
{
   public class Attachment
    {
        public Guid AttachmentId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        // Foreign key
        public Guid BugId { get; set; }

        // Navigation properties
        public Bug Bug { get; set; }
    }
}
