using Application_Service.Common;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.RequestAndResponseModel.AuthenticationModels;
using Application_Service.RequestAndResponseModel.Pagination;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Domain_Service.Entities.Identity;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application_Service.Services.UserManagmentServices.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<ApiResponse<PaginationResponse<GetUserDto>>> GetAllUsers(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var query = _uow.UserRepo.Query()
                    .Include(u => u.Department)
                    .AsNoTracking();

                var totalRecords = await query.CountAsync();

                var users = await query
                    .OrderByDescending(u => u.CreatedAt)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var roles = await _uow.UserRoleRepo.GetAllAsync();
                var roleDict = roles.ToDictionary(r => r.UserId, r => r.RoleName);

                var result = users.Select(user => new GetUserDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Contact = user.Contact,
                    CNIC = user.CNIC,
                    ImageUrl = user.ImageUrl,
                    Department = user.Department?.Name ?? string.Empty,
                    DepartmentId = user.DepartmentId,
                    Role = roleDict.TryGetValue(user.UserId, out var role) ? role : default,
                    Status = user.Status,
                    CreatedAt = user.CreatedAt
                }).ToList();

                return ApiResponse<PaginationResponse<GetUserDto>>.Success(
                    new PaginationResponse<GetUserDto>
                    {
                        Items = result,
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        TotalRecords = totalRecords
                    },
                    "Users retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<PaginationResponse<GetUserDto>>.Fail(
                    "Failed to retrieve users",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<PaginationResponse<GetUserDto>>> GetUsersByFilter(GetUserByRoleAndStatusRequest request)
        {
            try
            {
                var query = _uow.UserRepo.Query()
                    .Include(u => u.Department)
                    .AsNoTracking();

                // 🔹 Filter by status (DB side)
                if (request.status.HasValue)
                {
                    query = query.Where(u => u.Status == request.status.Value);
                }

                // 🔹 Filter by role (DB optimized)
                if (request.role.HasValue)
                {
                    var userIds = await _uow.UserRoleRepo.Query()
                        .Where(r => r.RoleName == request.role.Value)
                        .Select(r => r.UserId)
                        .ToListAsync();

                    query = query.Where(u => userIds.Contains(u.UserId));
                }

                var totalRecords = await query.CountAsync();

                var users = await query
                    .OrderByDescending(u => u.CreatedAt)
                    .Skip((request.pageNumber - 1) * request.pageSize)
                    .Take(request.pageSize)
                    .ToListAsync();

                var roles = await _uow.UserRoleRepo.GetAllAsync();
                var roleDict = roles.ToDictionary(r => r.UserId, r => r.RoleName);

                var result = users.Select(user => new GetUserDto
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Email = user.Email,
                    Contact = user.Contact,
                    CNIC = user.CNIC,
                    ImageUrl = user.ImageUrl,
                    Department = user.Department?.Name ?? string.Empty,
                    DepartmentId = user.DepartmentId,
                    Role = roleDict.TryGetValue(user.UserId, out var role) ? role : default,
                    Status = user.Status,
                    CreatedAt = user.CreatedAt
                }).ToList();

                return ApiResponse<PaginationResponse<GetUserDto>>.Success(
                    new PaginationResponse<GetUserDto>
                    {
                        Items = result,
                        PageNumber = request.pageNumber,
                        PageSize = request.pageSize,
                        TotalRecords = totalRecords
                    },
                    "Filtered users retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<PaginationResponse<GetUserDto>>.Fail(
                    "Failed to retrieve users",
                    ResponseType.InternalServerError);
            }
        }
        public async Task<ApiResponse<string>> UpdateUser(UpdateUserDto request)
        {
            try
            {
                // 🔹 1. Check user exists
                var user = await _uow.UserRepo.FirstOrDefaultAsync(
                    u => u.UserId == request.UserId
                );

                if (user == null)
                {
                    return ApiResponse<string>.Fail(
                        "User not found",
                        ResponseType.NotFound);
                }

                // 🔹 2. Duplicate validation
                var duplicateUser = await _uow.UserRepo.FirstOrDefaultAsync(u =>
                    (u.Email == request.Email ||
                     u.UserName == request.UserName ||
                     u.CNIC == request.CNIC)
                    && u.UserId != request.UserId
                );

                if (duplicateUser != null)
                {
                    var errors = new List<string>();

                    if (duplicateUser.Email == request.Email)
                        errors.Add("Email already registered");

                    if (duplicateUser.UserName == request.UserName)
                        errors.Add("Username already taken");

                    if (duplicateUser.CNIC == request.CNIC)
                        errors.Add("CNIC already exists");

                    return ApiResponse<string>.Fail(
                        string.Join(" | ", errors),
                        ResponseType.Conflict);
                }

                // 🔹 3. Validate Department
                if (request.DepartmentId.HasValue)
                {
                    var department = await _uow.DepartmentRepository
                        .GetByIdAsync(request.DepartmentId.Value);

                    if (department == null)
                    {
                        return ApiResponse<string>.Fail(
                            "Invalid department",
                            ResponseType.BadRequest);
                    }
                }

                // 🔥 4. Transaction Start
                await _uow.ExecuteInTransactionAsync(async () =>
                {
                    // 🔹 Update User
                    user.FullName = request.FullName;
                    user.UserName = request.UserName;
                    user.Email = request.Email;
                    user.Contact = request.Contact;
                    user.CNIC = request.CNIC;
                    user.DepartmentId = request.DepartmentId;
                    user.Status = request.Status;

                    await _uow.UserRepo.Update(user);

                    // 🔹 Update Role
                    var userRole = await _uow.UserRoleRepo.FirstOrDefaultAsync(
                        ur => ur.UserId == request.UserId
                    );

                    if (userRole != null)
                    {
                        userRole.RoleName = request.Role;
                        await _uow.UserRoleRepo.Update(userRole);
                    }
                    else
                    {
                        await _uow.UserRoleRepo.CreateAsync(new UserRole
                        {
                            UserId = user.UserId,
                            RoleName = request.Role
                        });
                    }
                });

                return ApiResponse<string>.Success("UpdateUser",
                    "User updated successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<string>.Fail(
                    "Failed to update user",
                    ResponseType.InternalServerError);
            }
        }
    }
}
