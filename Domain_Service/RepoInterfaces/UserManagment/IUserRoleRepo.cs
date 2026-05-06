using Domain_Service.Entities.Identity;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.GenricRepo;

namespace Domain_Service.RepoInterfaces.UserManagment
{
    public interface IUserRoleRepo : IRepository<UserRole>
    {

        Task<List<RoleType>> GetUserRoleByUserId(Guid UserId);
    }




}
