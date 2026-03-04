using Application_Service.DTO_s.AnnouncementModelDTO_s.DownloadDTO;
using FluentValidation;

namespace Application_Service.DTO_s.Validators.AnnouncementModelValidators.DownloadValidators
{
    public class DownloadAddDtoValidator : AbstractValidator<DownloadAddDto>
    {
        public DownloadAddDtoValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // FileId rules
            // -------------------------------
            RuleFor(x => x.FileId)
                .NotEmpty().WithMessage("FileId is required.")
                .NotEqual(Guid.Empty).WithMessage("FileId cannot be empty.");

            // -------------------------------
            // Title rules
            // -------------------------------
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

            // -------------------------------
            // FilePath rules
            // -------------------------------
            RuleFor(x => x.FilePath)
                .NotEmpty().WithMessage("FilePath is required.")
                .MaximumLength(500).WithMessage("FilePath cannot exceed 500 characters.");

            // -------------------------------
            // UploadedBy rules
            // -------------------------------
            RuleFor(x => x.UploadedBy)
                .NotEmpty().WithMessage("UploadedBy is required.")
                .NotEqual(Guid.Empty).WithMessage("UploadedBy cannot be empty.");

            // -------------------------------
            // DateUploaded rules
            // -------------------------------
            RuleFor(x => x.DateUploaded)
                .NotEmpty().WithMessage("DateUploaded is required.")
                .GreaterThan(DateTime.MinValue)
                .WithMessage("DateUploaded must be a valid date.");
        }
    }
}