using Application_Service.DTO_s.StudentDTO_s.SemesterDTO_s;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.StudentModelValidators.SemesterValidators
{
    public class SemesterUpdateDtoValidator : AbstractValidator<SemesterUpdateDto>
    {
        public SemesterUpdateDtoValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // SemesterId rules
            // -------------------------------
            RuleFor(x => x.SemesterId)
                .NotEqual(Guid.Empty).WithMessage("SemesterId cannot be empty.")
                .When(x => x.SemesterId.HasValue);

            // -------------------------------
            // SessionId rules
            // -------------------------------
            RuleFor(x => x.SessionId)
                .NotEqual(Guid.Empty).WithMessage("SessionId cannot be empty.")
                .When(x => x.SessionId.HasValue);

            // -------------------------------
            // Name rules
            // -------------------------------
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Semester name cannot exceed 100 characters.")
                .Matches(@"^[A-Za-z0-9 ]+$")
                .WithMessage("Semester name can contain only letters and numbers.")
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            // -------------------------------
            // Year rules
            // -------------------------------
            RuleFor(x => x.Year)
                .GreaterThan(2000).WithMessage("Year must be greater than 2000.")
                .LessThanOrEqualTo(DateTime.Now.Year + 1)
                .WithMessage("Year cannot be in the far future.")
                .When(x => x.Year.HasValue);

            // -------------------------------
            // Date rules (Cross validation)
            // -------------------------------
            RuleFor(x => x)
                .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.EndDate > x.StartDate)
                .WithMessage("End date must be greater than Start date.");

            // -------------------------------
            // Students rules
            // -------------------------------
            RuleFor(x => x.Students)
                .NotNull().WithMessage("Students list cannot be null.")
                .When(x => x.Students != null);
        }
    }
}