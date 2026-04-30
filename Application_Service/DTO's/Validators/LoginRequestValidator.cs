using Application_Service.RequestAndResponseModel.AuthenticationModels;
using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;

namespace Application_Service.DTO_s.Validators
{
    public class LoginRequestValidator : AbstractValidator<UserLoginRequest>
    {
        public LoginRequestValidator()
        {
            // Stop validation on first failure at class level
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleLevelCascadeMode = CascadeMode.Stop;

            // -------------------------------
            // FullName rules
            // -------------------------------
            RuleFor(x => x.CNIC)
                .NotEmpty().WithMessage("CNIC is required.")
                .Matches(@"^\d{5}-\d{7}-\d{1}$")
                .WithMessage("CNIC must be in format 12345-1234567-1");

            // -------------------------------
            // Password rules
            // -------------------------------
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
