using FluentValidation;

namespace BugTicketingSystem.BL.Dtos.BugDtos.Validator
{
    public class BugValidator : AbstractValidator<BugCreateDto>
    {
        public BugValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required").WithErrorCode("BUG_TITLE_REQUIRED")
                .MinimumLength(5).WithMessage("Title must be at least 5 characters").WithErrorCode("BUG_TITLE_TOO_SHORT")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters").WithErrorCode("BUG_TITLE_TOO_LONG");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required").WithErrorCode("BUG_DESC_REQUIRED")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters").WithErrorCode("BUG_DESC_TOO_SHORT")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters").WithErrorCode("BUG_DESC_TOO_LONG");

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required").WithErrorCode("PROJECT_ID_REQUIRED");
        }
    }
}