using Application_Service.DTO_s.StudentDTO_s.SemesterDTO_s;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.StudentModelValidators.SemesterValidators
{
    public class SemesterAddDtoValidator : AbstractValidator<SemesterAddDto>
    {
        public SemesterAddDtoValidator()
        {
            // Stop validation on first failure
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // SemesterId rules
            // -------------------------------
            RuleFor(x => x.SemesterId)
                .NotEmpty().WithMessage("SemesterId is required.")
                .NotEqual(Guid.Empty).WithMessage("SemesterId cannot be empty.");

            // -------------------------------
            // SessionId rules
            // -------------------------------
            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("SessionId is required.")
                .NotEqual(Guid.Empty).WithMessage("SessionId cannot be empty.");

            // -------------------------------
            // Name rules
            // -------------------------------
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Semester name is required.")
                .MaximumLength(100).WithMessage("Semester name cannot exceed 100 characters.")
                .Matches(@"^[A-Za-z0-9 ]+$")
                .WithMessage("Semester name can contain only letters and numbers.");

            // -------------------------------
            // Year rules
            // -------------------------------
            RuleFor(x => x.Year)
                .GreaterThan(2000).WithMessage("Year must be greater than 2000.")
                .LessThanOrEqualTo(DateTime.Now.Year + 1)
                .WithMessage("Year cannot be in the far future.");

            // -------------------------------
            // StartDate rules
            // -------------------------------
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .GreaterThan(DateTime.MinValue)
                .WithMessage("Start date must be valid.");

            // -------------------------------
            // EndDate rules
            // -------------------------------
            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be greater than Start date.");

            // -------------------------------
            // Students rules
            // -------------------------------
            RuleFor(x => x.Students)
                .NotNull().WithMessage("Students list cannot be null.");
        }
    }
}