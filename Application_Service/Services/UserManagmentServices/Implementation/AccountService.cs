using Application_Service.Common;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Mapper_s.UserManagmentMappers;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.EmailRepo;
using Domain_Service.RepoInterfaces.UnitOfWork;

namespace Application_Service.Services.UserManagmentServices.Implementation
{
    public class AccountService : IAccounService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailRepository _emailService;
        public AccountService(IUnitOfWork unitOfWork, IEmailRepository emailRepository)
        {
            this._uow = unitOfWork;
            this._emailService = emailRepository;
        }
        public async Task<ApiResponse<CreateUserResponseDto>> CreateAccount(CreateUserDto request, RoleType role)
        {
            try
            {
               
                var existingUser = await _uow.UserRepo.FirstOrDefaultAsync(u =>
                    u.Email == request.Email ||
                    u.UserName == request.UserName ||
                    u.CNIC == request.CNIC);

                if (existingUser != null)
                {
                    var errors = new List<string>();

                    if (existingUser.Email == request.Email)
                        errors.Add("Email already registered");

                    if (existingUser.UserName == request.UserName)
                        errors.Add("Username already registered");

                    if (existingUser.CNIC == request.CNIC)
                        errors.Add("CNIC already registered");

                    return ApiResponse<CreateUserResponseDto>.Fail(
                        string.Join(" | ", errors),
                        ResponseType.Conflict);
                }

                // 🔹 Handle temp password (record → use `with`)
                if (request.GeneratTempPassword)
                {
                    request = request with
                    {
                        Password = PasswordGenerator.GenerateRandomPassword()
                    };
                }
                var user = request.MapToDomain();

                // 🔹 Create user inside transaction
                await _uow.ExecuteInTransactionAsync(async () =>
                {
                 
                    await _uow.UserRepo.CreateAsync(user);

                    var credential = user.MapToCreadDomain(request.Password);
                    await _uow.UserCreadentialRepo.CreateAsync(credential);

                    var userRole = user.MapToUserRoleDomain(role);
                    await _uow.UserRoleRepo.CreateAsync(userRole);
                });

                // 🔹 Send email AFTER successful commit
                if (request.SendWelcomeEmail)
                {
                    var emailSent = await _emailService.SendAccountCreatWelcomeEmail(
                        request.Email,
                        request.FullName,
                        request.UserName,
                        request.Password,
                        role.ToString()
                    );

                    if (!emailSent)
                    {
                       

                        return ApiResponse<CreateUserResponseDto>.Success(
                            user.MapToResponse(),
                            "Email Sending Failed But User created successfully",
                            ResponseType.Created);
                    }
                }

                return ApiResponse<CreateUserResponseDto>.Success(
                    user.MapToResponse(),
                    "User created successfully",
                    ResponseType.Created);
            }
            catch (Exception ex)
            {
                // 🔹 Log internally, don’t expose raw exception
                // _logger.LogError(ex, "Error creating user");

                return ApiResponse<CreateUserResponseDto>.Fail(
                    "Failed to create user",
                    ResponseType.InternalServerError);
            }
        }
    }
}
