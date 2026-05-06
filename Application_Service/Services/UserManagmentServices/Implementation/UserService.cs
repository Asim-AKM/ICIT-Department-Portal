using Application_Service.Common;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.RequestAndResponseModel.AuthenticationModels;
using Application_Service.RequestAndResponseModel.Pagination;
using Application_Service.Services.UserManagmentServices.Interfaces;
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
    }
}
