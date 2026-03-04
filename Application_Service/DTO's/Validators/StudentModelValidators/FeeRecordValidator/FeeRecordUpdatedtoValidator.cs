using Application_Service.DTO_s;
using Application_Service.DTO_s.StudentDTO_s.FeeRecordDTO_s;
using FluentValidation;

namespace Application_Service.DTO_s.Validators
{
    public class FeeRecordUpdateDtoValidator : AbstractValidator<FeeRecordUpdateDto>
    {
        public FeeRecordUpdateDtoValidator()
        {
            // Stop validation on first failure at class level
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // FeeId rules
            // -------------------------------
            RuleFor(x => x.FeeId)
                .NotEqual(Guid.Empty).WithMessage("FeeId cannot be empty.")
                .When(x => x.FeeId.HasValue);

            // -------------------------------
            // StudentId rules
            // -------------------------------
            RuleFor(x => x.StudentId)
                .NotEqual(Guid.Empty).WithMessage("StudentId cannot be empty.")
                .When(x => x.StudentId.HasValue);

            // -------------------------------
            // SemesterId rules
            // -------------------------------
            RuleFor(x => x.SemesterId)
                .NotEqual(Guid.Empty).WithMessage("SemesterId cannot be empty.")
                .When(x => x.SemesterId.HasValue);

            // -------------------------------
            // Amount rules
            // -------------------------------
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.")
                .When(x => x.Amount.HasValue);

            // -------------------------------
            // Status rules
            // -------------------------------
            RuleFor(x => x.Status)
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.")
                .Matches(@"^[A-Za-z ]+$")
                .WithMessage("Status must contain only letters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Status));

            // -------------------------------
            // DueDate rules
            // -------------------------------
            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.MinValue).WithMessage("Due date must be valid.")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Due date cannot be in the past.")
                .When(x => x.DueDate.HasValue);
        }
    }
}