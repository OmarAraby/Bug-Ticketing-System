using FluentValidation;


namespace BugTicketingSystem.BL.Dtos.UserDtos.Validator
{
    public class LoginValidator : AbstractValidator<UserLoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
                .Must(email => email.Contains("@")).WithMessage("Email must contain '@'")
                .Must(email =>
                {
                    var parts = email.Split('@');
                    return parts.Length == 2 && !string.IsNullOrEmpty(parts[0]) && parts[1].Contains(".");
                }).WithMessage("Invalid email structure (e.g., 'user@example.com')");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");
        }
    }
}

