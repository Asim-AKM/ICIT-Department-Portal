using Application_Service.DTO_s.FYPproposalDTO_s.FYPTeamDTO;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.FYPModelValidators.FYPTeamValidators
{
    public class FYPTeamUpdateDtoValidator : AbstractValidator<TeamUpdateDto>
    {
        public FYPTeamUpdateDtoValidator()
        {
            // Stop validation on first failure
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // TeamId rules
            // -------------------------------
            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("TeamId is required.")
                .NotEqual(Guid.Empty).WithMessage("TeamId cannot be empty.");

            // -------------------------------
            // TeamName rules
            // -------------------------------
            RuleFor(x => x.TeamName)
                .NotEmpty().WithMessage("Team name is required.")
                .MaximumLength(100).WithMessage("Team name cannot exceed 100 characters.")
                .Matches(@"^[A-Za-z0-9 ]+$")
                .WithMessage("Team name can contain only letters and numbers.");

            // -------------------------------
            // LeaderId rules
            // -------------------------------
            RuleFor(x => x.LeaderId)
                .NotEmpty().WithMessage("LeaderId is required.")
                .NotEqual(Guid.Empty).WithMessage("LeaderId cannot be empty.");

            // -------------------------------
            // Members rules
            // -------------------------------
            RuleFor(x => x.Members)
                .NotNull().WithMessage("Members list cannot be null.")
                .Must(m => m.Count > 0)
                .WithMessage("Team must have at least one member.");

            // -------------------------------
            // FacultyId rules
            // -------------------------------
            RuleFor(x => x.FacultyId)
                .NotEmpty().WithMessage("FacultyId is required.")
                .NotEqual(Guid.Empty).WithMessage("FacultyId cannot be empty.");
        }
    }
}