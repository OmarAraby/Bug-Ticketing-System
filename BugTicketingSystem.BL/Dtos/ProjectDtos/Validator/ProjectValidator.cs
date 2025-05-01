using FluentValidation;

namespace BugTicketingSystem.BL.Dtos.ProjectDtos.Validator
{
    public class ProjectValidator : AbstractValidator<ProjectCreateDto>
    {
        public ProjectValidator()
        {
            RuleFor(x => x.ProjectName)
                .NotEmpty().WithMessage("Project name is required").WithErrorCode("PROJECT_NAME_REQUIRED")
                .MinimumLength(3).WithMessage("Project name must be at least 3 characters").WithErrorCode("PROJECT_NAME_TOO_SHORT");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required").WithErrorCode("DESCRIPTION_REQUIRED");
        }
    }
}
