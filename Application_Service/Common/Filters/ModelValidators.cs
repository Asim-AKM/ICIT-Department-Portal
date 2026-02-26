using Application_Service.DTO_s.UserManagmentDTO_s;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Application_Service.Common.Filters
{
    public static class ModelValidators
    {
        public static IServiceCollection AddModelValidator(
         this IServiceCollection services)
        {
            services
                .AddValidatorsFromAssemblyContaining<CreateUserDto>()
                .AddFluentValidationAutoValidation();

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(x => x.Value!.Errors.Count > 0)
                            .Select(x => new
                            {
                                Field = x.Key,
                                Errors = x.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                            })
                            .ToList();

                        return new BadRequestObjectResult(
                            ApiResponse<object>.Fail(
                                errors,
                                "Validation Errors"
                            )
                        );
                    };
                });

            return services;
        }
    }
}
