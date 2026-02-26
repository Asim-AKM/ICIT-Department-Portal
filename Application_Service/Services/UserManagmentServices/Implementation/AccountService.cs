using Application_Service.Common;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Mapper_s.UserManagmentMappers;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;

namespace Application_Service.Services.UserManagmentServices.Implementation
{
    public class AccountService : IAccounService
    {
        private readonly IUnitOfWork _uow;
        public AccountService(IUnitOfWork unitOfWork)
        {
          
             this._uow = unitOfWork;
        }
        public async Task<ApiResponse<CreateUserDto>> CreateAccount(CreateUserDto request,RoleType role)
        {
            var emailExistance = await _uow.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            var userNameExistance = await _uow.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);

            // Acomulate Errors in a list 
            if (emailExistance != null || userNameExistance != null)
            {
                List<string> errorsList = new List<string>();
                if (emailExistance != null)
                    errorsList.Add("Email Already Exist");
                if (userNameExistance != null)
                    errorsList.Add("UserName Already Exist");

                var error = string.Join(" | ", errorsList);
                return ApiResponse<CreateUserDto>.Fail(error, ResponseType.Conflict);
            }

            // Create User
            var user = request.MapToDomain();
            await _uow.Users.CreateAsync(user);
            var cread = user.MapToCreadDomain(request.Password);
            await _uow.UserCreads.CreateAsync(cread);
            await _uow.UserRoles.CreateAsync(user.MapToUserRoleDomain(role));

            //Save to database
            var saved = await _uow.SaveChangesAsync() > 0;

            if (!saved)
            {
                return ApiResponse<CreateUserDto>.Fail("Failed to Create User", ResponseType.BadRequest);
            }

            return ApiResponse<CreateUserDto>.Success(request,"User created successfully",ResponseType.Created);
        }
    }
}
