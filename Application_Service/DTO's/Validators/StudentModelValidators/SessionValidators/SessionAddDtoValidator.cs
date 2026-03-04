using Application_Service.DTO_s.StudentDTO_s;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.StudentModelValidators.SessionValidators
{
    public class SessionAddDtoValidator : AbstractValidator<SessionAddDto>
    {
        public SessionAddDtoValidator()
        {
            // Stop validation on first failure
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // Name rules
            // -------------------------------
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Session name is required.")
                .MaximumLength(100).WithMessage("Session name cannot exceed 100 characters.")
                .Matches(@"^[A-Za-z0-9 ]+$")
                .WithMessage("Session name can contain only letters and numbers.");

            // -------------------------------
            // StartYear rules
            // -------------------------------
            RuleFor(x => x.StartYear)
                .NotEmpty().WithMessage("Start year is required.")
                .GreaterThan(DateTime.MinValue)
                .WithMessage("Start year must be valid.");

            // -------------------------------
            // EndYear rules
            // -------------------------------
            RuleFor(x => x.EndYear)
                .NotEmpty().WithMessage("End year is required.")
                .GreaterThan(x => x.StartYear)
                .WithMessage("End year must be greater than Start year.");
        }
    }
}