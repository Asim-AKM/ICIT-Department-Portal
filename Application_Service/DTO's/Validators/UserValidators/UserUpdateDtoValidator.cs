using Application_Service.DTO_s.UserManagmentDTO_s;
using FluentValidation;

namespace Application_Service.DTO_s.Validators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            // Stop validation on first failure at class level
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // FullName rules
            // -------------------------------
            RuleFor(x => x.FullName)
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.")
                .Matches(@"^[A-Za-z ]+$")
                .WithMessage("Full name must contain only letters.")
                .When(x => !string.IsNullOrWhiteSpace(x.FullName));

            // -------------------------------
            // UserName rules
            // -------------------------------
            RuleFor(x => x.UserName)
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.UserName));

            // -------------------------------
            // Email rules
            // -------------------------------
            RuleFor(x => x.Email)
                 .EmailAddress().WithMessage("Invalid email format.")
                 .Matches(@"^[^@\s]+(\.[^@\s]+)?@[^@\s]+\.[^@\s]+$")
                 .WithMessage("Email can contain only one dot before the @.")
                 .When(x => !string.IsNullOrWhiteSpace(x.Email));

            // -------------------------------
            // Password rules
            // -------------------------------
            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
                .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.")
                .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=]+")
                .WithMessage("Password must contain at least one special character (!@#$%^&*()-+=).")
                .When(x => !string.IsNullOrWhiteSpace(x.Password));

            // -------------------------------
            // Role rules
            // -------------------------------
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Role must be a valid value: Admin, User, or Manager.");
        }
    }
}