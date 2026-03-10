using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.RequestAndResponseModel.StudentModels;
using FluentValidation;

namespace Application_Service.DTO_s.Validators
{
    public class GetStudentSessionRequestValidator : AbstractValidator<GetStudentBySessionRequest>
    {
        public GetStudentSessionRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.SessionId)
                .NotEqual(Guid.Empty)
                .WithMessage("SessionId is required");

            RuleFor(x => x.StudentStatus)
                .NotNull().WithMessage("Student Status Required")
                .IsInEnum().WithMessage("Invalid Student Status");

        }
    }
}
