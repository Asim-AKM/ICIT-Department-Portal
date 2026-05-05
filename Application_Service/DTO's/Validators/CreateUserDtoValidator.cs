using Application_Service.DTO_s.UserManagmentDTO_s;
using FluentValidation;

namespace Application_Service.DTO_s.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            // Stop validation on first failure at class level
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // FullName rules
            // -------------------------------
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100).WithMessage("Full name cannot exceed 100 characters.")
                .Matches(@"^[A-Za-z ]+$")
                .WithMessage("Full name must contain only letters");

            // -------------------------------
            // UserName rules
            // -------------------------------
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            // -------------------------------
            // Email rules
            // -------------------------------
            RuleFor(x => x.Email)
                 .NotEmpty().WithMessage("Email is required.")
                 .EmailAddress().WithMessage("Invalid email format.")
                 .Matches(@"^[^@\s]+(\.[^@\s]+)?@[^@\s]+\.[^@\s]+$")
                 .WithMessage("Email can contain only one dot before the @.");


            //// -------------------------------
            //// Password rules
            //// -------------------------------
            //RuleFor(x => x.Password)
            //    .NotEmpty().WithMessage("Password is required.")
            //    .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            //    .MaximumLength(100).WithMessage("Password cannot exceed 100 characters.")
            //    .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            //    .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            //    .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
            //    .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=]+").WithMessage("Password must contain at least one special character (!@#$%^&*()-+=)");

            // -------------------------------
            // Role rules
            // -------------------------------
            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Role must be a valid value: Admin, User, or Manager.");
            //
            // CNIC Rules
            // -------------------------------
            RuleFor(x => x.CNIC)
                .NotEmpty().WithMessage("CNIC is required.")
                .Matches(@"^\d{5}-\d{7}-\d{1}$")
                .WithMessage("CNIC must be in format 12345-1234567-1");

        }
    }
}
