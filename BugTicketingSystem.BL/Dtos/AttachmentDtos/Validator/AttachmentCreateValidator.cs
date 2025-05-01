using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTicketingSystem.BL.Dtos.AttachmentDtos.Validator
{
    public class AttachmentCreateValidator : AbstractValidator<AttachmentCreateDto>
    {
        public AttachmentCreateValidator()
        {
            RuleFor(dto => dto.BugId)
                .NotEmpty().WithMessage("BugId must be a valid GUID")
                .NotEqual(Guid.Empty).WithMessage("BugId must be a valid GUID");

            RuleFor(dto => dto.FileUrl)
                .NotEmpty().WithMessage("File URL is required")
                .Must(url => Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute)).WithMessage("File URL must be a valid URL");

            RuleFor(dto => dto.FileName)
                .NotEmpty().WithMessage("File name is required");
        }
    }
}
