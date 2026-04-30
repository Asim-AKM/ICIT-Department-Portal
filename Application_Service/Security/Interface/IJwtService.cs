using Domain_Service.Entities.Identity;
using Domain_Service.Enum;

namespace Application_Service.Security.Interface
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(User user, List<RoleType> roles);

    }
}
