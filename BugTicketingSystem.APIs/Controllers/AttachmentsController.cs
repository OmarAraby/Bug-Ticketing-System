using BugTicketingSystem.APIs.HandleFiles;
using BugTicketingSystem.BL.Dtos.AttachmentDtos;
using BugTicketingSystem.BL.Managers.Attachment;
using BugTicketingSystem.BL.Utils.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BugTicketingSystem.APIs.Controllers
{
    [Route("api/bugs/{bugId}/[controller]")]
    [ApiController]
    [Authorize]
    public class AttachmentsController : ControllerBase
    {
        private readonly IAttachmentManager _attachmentManager;
        private readonly IFileService _fileService;

        public AttachmentsController(IAttachmentManager attachmentManager, IFileService fileService)
        {
            _attachmentManager = attachmentManager;
            _fileService = fileService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<Results<Ok<APIResult<AttachmentDto>>, BadRequest<APIResult<AttachmentDto>>>> UploadAttachment(
            Guid bugId,
            [FromForm] FileUploadRequest fileRequest)
        {
            try
            {
                if (fileRequest == null || fileRequest.File == null || fileRequest.File.Length == 0)
                {
                    return TypedResults.BadRequest(new APIResult<AttachmentDto>
                    {
                        Success = false,
                        Errors = new[] { new APIError { Code = "FILE_NULL", Message = "No file was provided or file is empty" } }
                    });
                }

                var fileUploadResult = await _fileService.UploadFileAsync(fileRequest.File);
                var attachmentDto = new AttachmentCreateDto
                {
                    FileUrl = fileUploadResult.FileUrl,
                    FileName = fileRequest.File.FileName,
                    BugId = bugId
                };

                var result = await _attachmentManager.UploadAttachment(bugId, attachmentDto);
                return result.Success
                    ? TypedResults.Ok(result)
                    : TypedResults.BadRequest(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest(new APIResult<AttachmentDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "FILE_UPLOAD_ERROR", Message = ex.Message } }
                });
            }
        }

        [HttpGet]
        public async Task<Results<Ok<APIResult<IEnumerable<AttachmentDto>>>, NotFound<APIResult<IEnumerable<AttachmentDto>>>>> GetAttachmentsForBug(Guid bugId)
        {
            var result = await _attachmentManager.GetAttachmentsForBug(bugId);
            return result.Success
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }

        [HttpDelete("{attachmentId}")]
        public async Task<Results<Ok<APIResult>, NotFound<APIResult>, BadRequest<APIResult>>> DeleteAttachment(Guid bugId, Guid attachmentId)
        {
            var result = await _attachmentManager.DeleteAttachment(bugId, attachmentId);
            if (!result.Success)
            {
                if (result.Errors.Any(e => e.Code == "BUG_NOT_FOUND" || e.Code == "ATTACHMENT_NOT_FOUND"))
                {
                    return TypedResults.NotFound(result);
                }
                return TypedResults.BadRequest(result);
            }
            return TypedResults.Ok(result);
        }
    }
}