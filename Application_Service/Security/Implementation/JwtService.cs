using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Security.Interface;
using Domain_Service.Entities.Identity;
using Domain_Service.Enum;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application_Service.Security.Implementation
{
    public class JwtService : IJwtService
    {
        private readonly JWTSettings _jwtSettings;
        public JwtService(IOptions<JWTSettings> settings)
        {
            _jwtSettings = settings.Value;
        }
        public async Task<string> GenerateJwtToken(User user, List<RoleType> roles)
        {
            var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)
            };
            foreach (var role in roles)
            {
                claimsList.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var cread = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken
                (
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims:claimsList,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: cread);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
