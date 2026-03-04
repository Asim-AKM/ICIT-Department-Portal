using Application_Service.DTO_s.StudentDTO_s.Student;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.StudentModelValidators.StudentValidators
{
    public class StudentAddDtoValidator : AbstractValidator<StudentAddDto>
    {
        public StudentAddDtoValidator()
        {
            // Stop validation on first failure
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // StudentId rules
            // -------------------------------
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required.")
                .NotEqual(Guid.Empty).WithMessage("StudentId cannot be empty.");

            // -------------------------------
            // UserId rules
            // -------------------------------
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .NotEqual(Guid.Empty).WithMessage("UserId cannot be empty.");

            // -------------------------------
            // RegistrationNo rules
            // -------------------------------
            RuleFor(x => x.RegistrationNo)
                .NotEmpty().WithMessage("Registration number is required.")
                .MaximumLength(50).WithMessage("Registration number cannot exceed 50 characters.")
                .Matches(@"^[A-Za-z0-9\-\/]+$")
                .WithMessage("Registration number can contain only letters, numbers, - and /.");

            // -------------------------------
            // RollNo rules
            // -------------------------------
            RuleFor(x => x.RollNo)
                .NotEmpty().WithMessage("Roll number is required.")
                .MaximumLength(50).WithMessage("Roll number cannot exceed 50 characters.")
                .Matches(@"^[A-Za-z0-9\-]+$")
                .WithMessage("Roll number can contain only letters, numbers and -.");

            // -------------------------------
            // SamesterId rules
            // -------------------------------
            RuleFor(x => x.SamesterId)
                .NotEmpty().WithMessage("SemesterId is required.")
                .NotEqual(Guid.Empty).WithMessage("SemesterId cannot be empty.");

            // -------------------------------
            // SessionId rules
            // -------------------------------
            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("SessionId is required.")
                .NotEqual(Guid.Empty).WithMessage("SessionId cannot be empty.");

            // -------------------------------
            // GPA rules
            // -------------------------------
            RuleFor(x => x.GPA)
                .InclusiveBetween(0, 4)
                .WithMessage("GPA must be between 0 and 4.");

            // -------------------------------
            // FeeRecords rules
            // -------------------------------
            RuleFor(x => x.FeeRecords)
                .NotNull().WithMessage("Fee records list cannot be null.");
        }
    }
}