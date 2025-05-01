using FluentValidation;

namespace BugTicketingSystem.BL.Dtos.BugDtos.Validator
{

        public class AssignUserToBugDtoValidator : AbstractValidator<AssignUserToBugDto>
        {
            public AssignUserToBugDtoValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("UserId is required").WithErrorCode("USERID_REQUIRED")
                    .Must(id => id != Guid.Empty).WithMessage("UserId must be a valid GUID").WithErrorCode("INVALID_USERID");

                RuleFor(x => x.Role)
                    .IsInEnum().WithMessage("Role must be a valid RoleType value").WithErrorCode("INVALID_ROLE");
            }
        }
    }

