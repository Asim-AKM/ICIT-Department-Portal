using Application_Service.DTO_s;
using Application_Service.DTO_s.StudentDTO_s.FeeRecordDTO_s;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.StudentModelValidators.FeeRecordValidator
{
    public class FeeRecordAddDtoValidator : AbstractValidator<FeeRecordAddDto>
    {
        public FeeRecordAddDtoValidator()
        {
            // Stop validation on first failure at class level
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // FeeId rules
            // -------------------------------
            RuleFor(x => x.FeeId)
                .NotEmpty().WithMessage("FeeId is required.")
                .NotEqual(Guid.Empty).WithMessage("FeeId cannot be empty.");

            // -------------------------------
            // StudentId rules
            // -------------------------------
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("StudentId is required.")
                .NotEqual(Guid.Empty).WithMessage("StudentId cannot be empty.");

            // -------------------------------
            // SemesterId rules
            // -------------------------------
            RuleFor(x => x.SemesterId)
                .NotEmpty().WithMessage("SemesterId is required.")
                .NotEqual(Guid.Empty).WithMessage("SemesterId cannot be empty.");

            // -------------------------------
            // Amount rules
            // -------------------------------
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            // -------------------------------
            // Status rules
            // -------------------------------
            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.")
                .Matches(@"^[A-Za-z ]+$")
                .WithMessage("Status must contain only letters.");

            // -------------------------------
            // DueDate rules
            // -------------------------------
            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage("Due date is required.")
                .GreaterThan(DateTime.MinValue).WithMessage("Due date must be valid.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Due date cannot be in the past.");
        }
    }
}