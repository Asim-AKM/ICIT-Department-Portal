using Application_Service.DTO_s.FYPproposalDTO_s.ProjectDTO;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.FYPModelValidators.ProjectValidators
{
    public class ProjectUpdateDtoValidator : AbstractValidator<ProjectUpdateDto>
    {
        public ProjectUpdateDtoValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("ProjectId is required.")
                .NotEqual(Guid.Empty).WithMessage("ProjectId cannot be empty.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Project title is required.")
                .MaximumLength(150).WithMessage("Project title cannot exceed 150 characters.");

            RuleFor(x => x.FacultyId)
                .NotEmpty().WithMessage("FacultyId is required.")
                .NotEqual(Guid.Empty).WithMessage("FacultyId cannot be empty.");

            RuleFor(x => x.Proposals)
                .NotNull().WithMessage("Proposals list cannot be null.");
        }
    }
}