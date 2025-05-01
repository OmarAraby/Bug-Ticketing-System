using BugTicketingSystem.BL.Dtos.AttachmentDtos;
using BugTicketingSystem.BL.Utils.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Managers.Attachment
{
   public interface IAttachmentManager
    {
        // File Management Methods
        Task<APIResult<AttachmentDto>> UploadAttachment(Guid bugId, AttachmentCreateDto dto);
        Task<APIResult<IEnumerable<AttachmentDto>>> GetAttachmentsForBug(Guid bugId);
        Task<APIResult> DeleteAttachment(Guid bugId, Guid attachmentId);
    }
}
