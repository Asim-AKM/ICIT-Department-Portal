
using Application_Service.DTO_s.FYPproposalDTO_s.ProposalDTO;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.FYPModelValidators.ProposalValidator
{

    public class ProposalUpdateDtoValidator : AbstractValidator<ProposalUpdateDto>
    {
        public ProposalUpdateDtoValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.ProposalId)
                .NotEmpty().WithMessage("ProposalId is required.")
                .NotEqual(Guid.Empty).WithMessage("ProposalId cannot be empty.");

            RuleFor(x => x.TeamId)
                .NotEmpty().WithMessage("TeamId is required.")
                .NotEqual(Guid.Empty).WithMessage("TeamId cannot be empty.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Proposal title is required.")
                .MaximumLength(150).WithMessage("Proposal title cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .MaximumLength(50).WithMessage("Status cannot exceed 50 characters.");

            RuleFor(x => x.SubmissionDate)
                .NotEmpty().WithMessage("Submission date is required.");
        }
    }
}
