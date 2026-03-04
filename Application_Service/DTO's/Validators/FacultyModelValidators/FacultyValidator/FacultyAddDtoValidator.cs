using Application_Service.DTO_s.FacultyModelDTO_s.FacultyDTO;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.FacultyModelValidators.FacultyValidator
{
    public class FacultyAddDtoValidator : AbstractValidator<FacultyAddDto>
    {
        public FacultyAddDtoValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.FacultyId)
                .NotEmpty().WithMessage("FacultyId is required.")
                .NotEqual(Guid.Empty).WithMessage("FacultyId cannot be empty.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .NotEqual(Guid.Empty).WithMessage("UserId cannot be empty.");

            RuleFor(x => x.Department)
                .NotEmpty().WithMessage("Department is required.")
                .MaximumLength(100).WithMessage("Department cannot exceed 100 characters.");

            RuleFor(x => x.SubjectsTaught)
                .NotNull().WithMessage("Subjects list cannot be null.");

            RuleFor(x => x.SupervisedProjects)
                .NotNull().WithMessage("Projects list cannot be null.");
        }
    }
}