using BugTicketingSystem.BL.Dtos.AttachmentDtos;
using BugTicketingSystem.BL.Utils.Error;
using BugTicketingSystem.DL.UnitOfWork;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Managers.Attachment
{
    public class AttachmentManager : IAttachmentManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AttachmentCreateDto> _attachmentCreateValidator;

        public AttachmentManager(
            IUnitOfWork unitOfWork,
            IValidator<AttachmentCreateDto> attachmentCreateValidator)
        {
            _unitOfWork = unitOfWork;
            _attachmentCreateValidator = attachmentCreateValidator;
        }

        public async Task<APIResult<AttachmentDto>> UploadAttachment(Guid bugId, AttachmentCreateDto dto)
        {
            // Validate bugId
            if (bugId == Guid.Empty)
            {
                return new APIResult<AttachmentDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "INVALID_BUGID", Message = "BugId must be a valid GUID" } }
                };
            }

            // Validate DTO
            var validationResult = await _attachmentCreateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new APIResult<AttachmentDto>
                {
                    Success = false,
                    Errors = validationResult.Errors.Select(e => new APIError
                    {
                        Code = e.ErrorCode ?? "VALIDATION_ERROR",
                        Message = e.ErrorMessage
                    }).ToArray()
                };
            }

            // Check if bug exists
            var bug = await _unitOfWork.BugRepository.GetByIdAsync(bugId);
            if (bug == null)
            {
                return new APIResult<AttachmentDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "BUG_NOT_FOUND", Message = "Bug not found" } }
                };
            }

            try
            {
                // Create attachment entity
                var attachment = new DL.Models.Attachment
                {
                    AttachmentId = Guid.NewGuid(),
                    FileName = dto.FileName,
                    FilePath = dto.FileUrl, // Use the URL provided by the Controller (e.g., /api/static-files/some-guid.jpg)
                    UploadedAt = DateTime.UtcNow,
                    BugId = bugId
                };

                // Save to database
                await _unitOfWork.AttachmentRepository.AddAsync(attachment);
                await _unitOfWork.SaveChangesAsync();

                return new APIResult<AttachmentDto>
                {
                    Success = true,
                    Data = MapToAttachmentDto(attachment)
                };
            }
            catch (Exception ex)
            {
                return new APIResult<AttachmentDto>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "SYS_ERROR", Message = ex.Message } }
                };
            }
        }

        public async Task<APIResult<IEnumerable<AttachmentDto>>> GetAttachmentsForBug(Guid bugId)
        {
            // Validate bugId
            if (bugId == Guid.Empty)
            {
                return new APIResult<IEnumerable<AttachmentDto>>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "INVALID_BUGID", Message = "BugId must be a valid GUID" } }
                };
            }

            // Check if bug exists
            var bug = await _unitOfWork.BugRepository.GetByIdAsync(bugId);
            if (bug == null)
            {
                return new APIResult<IEnumerable<AttachmentDto>>
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "BUG_NOT_FOUND", Message = "Bug not found" } }
                };
            }

            var attachments = await _unitOfWork.AttachmentRepository.GetByBugIdAsync(bugId);
            return new APIResult<IEnumerable<AttachmentDto>>
            {
                Success = true,
                Data = attachments.Select(MapToAttachmentDto)
            };
        }

        public async Task<APIResult> DeleteAttachment(Guid bugId, Guid attachmentId)
        {
            // Validate bugId
            if (bugId == Guid.Empty)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "INVALID_BUGID", Message = "BugId must be a valid GUID" } }
                };
            }

            // Validate attachmentId
            if (attachmentId == Guid.Empty)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "INVALID_ATTACHMENTID", Message = "AttachmentId must be a valid GUID" } }
                };
            }

            // Check if bug exists
            var bug = await _unitOfWork.BugRepository.GetByIdAsync(bugId);
            if (bug == null)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "BUG_NOT_FOUND", Message = "Bug not found" } }
                };
            }

            // Check if attachment exists and belongs to the bug
            var attachment = await _unitOfWork.AttachmentRepository.GetByIdAsync(attachmentId);
            if (attachment == null || attachment.BugId != bugId)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "ATTACHMENT_NOT_FOUND", Message = "Attachment not found or does not belong to this bug" } }
                };
            }

            try
            {
                // Delete the file from the server
                var fileName = Path.GetFileName(attachment.FilePath); // Extract file name from URL (e.g., some-guid.jpg)
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload", fileName); // Match the folder in Program.cs
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                // Delete from database
                await _unitOfWork.AttachmentRepository.DeleteAsync(attachment);
                await _unitOfWork.SaveChangesAsync();

                return new APIResult
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new APIResult
                {
                    Success = false,
                    Errors = new[] { new APIError { Code = "SYS_ERROR", Message = "An error occurred while deleting the attachment" } }
                };
            }
        }

        private AttachmentDto MapToAttachmentDto(DL.Models.Attachment attachment)
        {
            return new AttachmentDto
            {
                AttachmentId = attachment.AttachmentId,
                FileName = attachment.FileName,
                FilePath = attachment.FilePath, // This will be a URL like /api/static-files/some-guid.jpg
                UploadedAt = attachment.UploadedAt,
                BugId = attachment.BugId
            };
        }
    }
}
