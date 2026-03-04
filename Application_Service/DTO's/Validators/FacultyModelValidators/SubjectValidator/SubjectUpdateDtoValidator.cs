using Application_Service.DTO_s.FacultyModelDTO_s.SubjectDTO;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.FacultyModelValidators.SubjectValidator
{
    public class SubjectUpdateDtoValidator : AbstractValidator<SubjectUpdateDto>
    {
        public SubjectUpdateDtoValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.SubjectId)
                .NotEmpty().WithMessage("SubjectId is required.")
                .NotEqual(Guid.Empty).WithMessage("SubjectId cannot be empty.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Subject title is required.")
                .MaximumLength(150).WithMessage("Subject title cannot exceed 150 characters.");

            RuleFor(x => x.SemesterId)
                .NotEmpty().WithMessage("SemesterId is required.")
                .NotEqual(Guid.Empty).WithMessage("SemesterId cannot be empty.");

            RuleFor(x => x.FacultyId)
                .NotEmpty().WithMessage("FacultyId is required.")
                .NotEqual(Guid.Empty).WithMessage("FacultyId cannot be empty.");
        }
    }
}