namespace BugTicketingSystem.APIs.HandleFiles
{
    public interface IFileService
    {
        Task<FileUploadResult> UploadFileAsync(IFormFile file);
    }
}
