using Microsoft.AspNetCore.Http;


namespace BugTicketingSystem.BL.Dtos.AttachmentDtos
{
    public class AttachmentCreateDto
    {
        public string FileUrl { get; set; } // URL of the uploaded file
        public string FileName { get; set; } // Name of the file
        public Guid BugId { get; set; } 
    }
}
