using Application_Service.DTO_s.AnnouncementModelDTO_s.AnnoncementDTO;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.AnnouncementModelValidators.AnnouncementValidator
{
    public class AnnouncementUpdateDtoValidator : AbstractValidator<AnnouncementUpdateDto>
    {
        public AnnouncementUpdateDtoValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.AnnouncementId)
                .NotEmpty().WithMessage("AnnouncementId is required.")
                .NotEqual(Guid.Empty).WithMessage("AnnouncementId cannot be empty.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(2000).WithMessage("Content cannot exceed 2000 characters.");

            RuleFor(x => x.PostedBy)
                .NotEmpty().WithMessage("PostedBy is required.")
                .NotEqual(Guid.Empty).WithMessage("PostedBy cannot be empty.");

            RuleFor(x => x.DatePosted)
                .NotEmpty().WithMessage("DatePosted is required.")
                .GreaterThan(DateTime.MinValue)
                .WithMessage("DatePosted must be a valid date.");
        }
    }
}