using Application_Service.DTO_s.UserManagmentDTO_s;
using Domain_Service.Entities.Identity;
using Domain_Service.Enum;

namespace Application_Service.Mapper_s.UserManagmentMappers
{
    public static class UserMapper
    {
        public static User MapToDomain(this CreateUserDto createUserDto)
        {
            return new User
            {
                UserId = Guid.NewGuid(),
                FullName = createUserDto.FullName,
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                Contact = string.Empty, // Default value, can be updated later
                ImageUrl = string.Empty, // Default value, can be updated later
                Status = UserStatus.Active, //Defual status for new users
            };
        }

    }
}
