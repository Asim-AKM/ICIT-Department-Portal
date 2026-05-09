using Application_Service.Common;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Mapper_s.UserManagmentMappers;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Domain_Service.Entities.Academic;
using Domain_Service.Entities.Identity;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.EmailRepo;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Application_Service.Services.UserManagmentServices.Implementation
{
    public class AccountService : IAccounService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailRepository _emailService;
        public AccountService(IUnitOfWork uow, IEmailRepository emailService)
        {
            _uow = uow;
            _emailService = emailService;
            Console.WriteLine($"UOW HashCode: {_uow.GetHashCode()}");
        }

        public async Task<ApiResponse<GetUserProfileDto>> GetProfileDetails(Guid userId)
        {
            try
            {
                // 🔹 Get user with Department (Eager Loading)
                var user = await _uow.UserRepo.FirstOrDefaultAsync(
                    u => u.UserId == userId,
                    include: q => q.Include(u => u.Department)
                );

                if (user == null)
                {
                    return ApiResponse<GetUserProfileDto>.Fail(
                        "User not found",
                        ResponseType.NotFound);
                }

                // 🔹 IMPORTANT: Make sure this is awaited (no Task leakage)
                var userRole = await _uow.UserRoleRepo.FirstOrDefaultAsync(
                    ur => ur.UserId == userId
                );

                // 🔹 Map safely (NO async objects inside)
                var response = new GetUserProfileDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Contact = user.Contact,
                    CNIC = user.CNIC,
                    CreatedAt = user.CreatedAt,
                    Role = userRole != null ? userRole.RoleName : default,
                    Department = user.Department != null ? user.Department.Name : string.Empty,
                    Email = user.Email,
                    ImageUrl = user.ImageUrl
                };

                return ApiResponse<GetUserProfileDto>.Success(
                    response,
                    "User profile retrieved successfully",
                    ResponseType.Ok);
            }
            catch (Exception)
            {
                return ApiResponse<GetUserProfileDto>.Fail(
                    "Failed to retrieve user profile",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<string>> RemoveProfileImage(Guid userId)
        {
            try
            {
                // 🔹 Get user
                var user = await _uow.UserRepo.FirstOrDefaultAsync(
                    u => u.UserId == userId
                );

                if (user == null)
                {
                    return ApiResponse<string>.Fail(
                        "User not found",
                        ResponseType.NotFound);
                }

                // 🔹 Check if image exists
                if (string.IsNullOrWhiteSpace(user.ImageUrl))
                {
                    return ApiResponse<string>.Fail(
                        "No profile image to delete",
                        ResponseType.BadRequest);
                }

                // 🔹 Delete file from disk
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

                try
                {
                    var fileName = Path.GetFileName(user.ImageUrl);
                    var fullPath = Path.Combine(uploadPath, fileName);

                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                catch
                {
                    // optional: log error, but don’t fail request
                }

                // 🔹 Remove from DB
                user.ImageUrl = string.Empty;

                await _uow.UserRepo.Update(user);
                await _uow.SaveChangesAsync();

                return ApiResponse<string>.Success(
                    "",
                    "Profile image removed successfully",
                    ResponseType.Ok);
            }
            catch (Exception)
            {
                return ApiResponse<string>.Fail(
                    "Failed to remove profile image",
                    ResponseType.InternalServerError);
            }
        }
        public async Task<ApiResponse<string>> UploadProfileImage(UserProfileImageUploadDto request)
        {
            try
            {
                // 🔹 1. Get user FIRST
                var user = await _uow.UserRepo.FirstOrDefaultAsync(
                    u => u.UserId == request.UserId
                );

                if (user == null)
                {
                    return ApiResponse<string>.Fail(
                        "User not found",
                        ResponseType.NotFound);
                }

                // 🔹 2. Save image (pass old image for deletion)
                var imageUrl = await SaveOrUpdateImageAsync(
                    request.file,
                    user.ImageUrl
                );

                // 🔹 3. Update user
                user.ImageUrl = imageUrl;

                await _uow.UserRepo.Update(user);
                await _uow.SaveChangesAsync(); // 🔥 IMPORTANT

                return ApiResponse<string>.Success(
                    imageUrl,
                    "Profile image updated successfully",
                    ResponseType.Ok);
            }
            catch (Exception)
            {
                return ApiResponse<string>.Fail(
                    "Failed to upload image",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<List<GetUserDto>>> GetAllUsers()
        {
            try
            {
                // 🔹 Get users with department
                var users = await _uow.UserRepo.GetAllAsync();

                if (users == null || !users.Any())
                {
                    return ApiResponse<List<GetUserDto>>.Success(
                        new List<GetUserDto>(),
                        "No users found",
                        ResponseType.Ok);
                }

                // 🔹 Get roles
                var roles = await _uow.UserRoleRepo.GetAllAsync();

                // 🔹 Map
                var result = users.Select(user =>
                {
                    var role = roles.FirstOrDefault(r => r.UserId == user.UserId);

                    return new GetUserDto
                    {
                        UserId = user.UserId,
                        FullName = user.FullName,
                        UserName = user.UserName,
                        Email = user.Email,
                        Contact = user.Contact,
                        CNIC = user.CNIC,
                        ImageUrl = user.ImageUrl,
                        Department = user.Department?.Name ?? string.Empty,
                        Role = role?.RoleName ?? default,
                        Status = user.Status,
                        CreatedAt = user.CreatedAt
                    };
                }).ToList();

                return ApiResponse<List<GetUserDto>>.Success(
                    result,
                    "Users retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<List<GetUserDto>>.Fail(
                    "Failed to retrieve users",
                    ResponseType.InternalServerError);
            }
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
                    if (existingUser.Email == request.Email) errors.Add("Email already registered");
                    if (existingUser.UserName == request.UserName) errors.Add("Username already registered");
                    if (existingUser.CNIC == request.CNIC) errors.Add("CNIC already registered");
                    return ApiResponse<CreateUserResponseDto>.Fail(string.Join(" | ", errors), ResponseType.Conflict);
                }

                if (request.GeneratTempPassword)
                {
                    request = request with { Password = PasswordGenerator.GenerateRandomPassword() };
                }

                var user = request.MapToDomain();

                await _uow.ExecuteInTransactionAsync(async () =>
                {
                    // 1. User create
                    await _uow.UserRepo.CreateAsync(user);

                    // 2. Credential create
                    var credential = user.MapToCreadDomain(request.Password);
                    await _uow.UserCreadentialRepo.CreateAsync(credential);

                    // 3. UserRole create
                    var userRole = user.MapToUserRoleDomain(role);
                    await _uow.UserRoleRepo.CreateAsync(userRole);

                    // 4. 🔴 SPECIFIC TABLE ENTRY - Separate method call
                    await CreateSpecificRoleEntry(user.UserId, request, role);
                });

                // Email send karna (transaction ke bahar)
                if (request.SendWelcomeEmail)
                {
                    var emailSent = await _emailService.SendAccountCreatWelcomeEmail(
                        request.Email, request.FullName, request.UserName,
                        request.Password, role.ToString()
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
                return ApiResponse<CreateUserResponseDto>.Fail(
                    "Failed to create user",
                    ResponseType.InternalServerError);
            }
        }

        //  PRIVATE METHOD - Isme saara role-specific logic hoga
        private async Task CreateSpecificRoleEntry(Guid userId, CreateUserDto request, RoleType role)
        {
            switch (role)
            {
                case RoleType.Student:
                    var student = new Student
                    {
                        StudentId = Guid.NewGuid(),
                        UserId = userId,
                        DepartmentId = request.DepartmentId ?? Guid.Empty,
                        StudentName = request.FullName,
                        StudentEmail = request.Email,
                        RollNo = "", // Separate method bana sakte ho
                        RegistrationNo = "", // Separate method
                        CNIC = request.CNIC,
                        GPA = 0,
                        SamesterId = Guid.Empty, // Aapki business logic
                        SessionId =Guid.Empty ,   // Aapki business logic
                        Status = StudentStatus.Unverified
                    };
                    await _uow.StudentRepo.CreateAsync(student);
                    break;

                case RoleType.Clerk:
                    var clerk = new Clerk
                    {
                        ClerkId = Guid.NewGuid(),
                        UserId = userId,
                        DepartmentId = request.DepartmentId ?? Guid.Empty,
                        Designation = "Clerk",
                        JoiningDate = DateTime.UtcNow
                    };
                    await _uow.ClerkRepo.CreateAsync(clerk);
                    break;

                case RoleType.Faculty:
                    var faculty = new Faculty
                    {
                        FacultyId = Guid.NewGuid(),
                        UserId = userId,
                        DepartmentId = request.DepartmentId ?? Guid.Empty,
                        Designation = "Faculty Member",
                        JoiningDate = DateTime.UtcNow
                    };
                    await _uow.FucaltyRepo.CreateAsync(faculty);
                    break;

                case RoleType.Admin:
                    // Admin ke liye kuch nahi karna
                    break;

                default:
                    throw new ArgumentException($"Unsupported role type: {role}");
            }
        }

        private async Task<string> SaveOrUpdateImageAsync(IFormFile file, string? oldImageUrl = null)
        {
            // 🔴 1. Null / empty check
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is required");

            // 🔴 2. File size validation (max 2MB)
            const long maxSize = 2 * 1024 * 1024;
            if (file.Length > maxSize)
                throw new Exception("File size must be less than 2MB");

            // 🔴 3. Extension validation
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Only JPG, JPEG, PNG files are allowed");

            // 🔴 4. MIME type validation (extra safety)
            var allowedMimeTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedMimeTypes.Contains(file.ContentType.ToLower()))
                throw new Exception("Invalid image format");

            // 🔹 Upload path
            var uploadPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "uploads"
            );

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // 🔹 Generate file name
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadPath, fileName);

            // 🔹 Save file
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 🔥 Delete old image safely
            if (!string.IsNullOrWhiteSpace(oldImageUrl))
            {
                try
                {
                    var oldFileName = Path.GetFileName(oldImageUrl);
                    var oldFilePath = Path.Combine(uploadPath, oldFileName);

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                catch
                {
                    // log if needed, don’t break flow
                }
            }

            var baseUrl = "https://localhost:5001"; 
            //return Url
            return $"{baseUrl}/uploads/{fileName}";
        }
    }
}
