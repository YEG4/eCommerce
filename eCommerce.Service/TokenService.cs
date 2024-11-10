using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eCommerce.Core.Entities.Identity;
using eCommerce.Core.JsonObjects;
using eCommerce.Core.Services;
using Microsoft.IdentityModel.Tokens;

namespace eCommerce.Service
{
    public class TokenService : ITokenServices
    {
        private readonly JwtOptions jwtOptions;
        public TokenService(JwtOptions jwtOptions)
        {
            this.jwtOptions = jwtOptions;

        }
        public async Task<string> GetAccessToken(AppUser user)
        {
            var tokenHander = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtOptions.Issuer,
                Audience = jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Name, user.DisplayName),
                })

            };
            var securityToken = tokenHander.CreateToken(tokenDescriptor);
            return tokenHander.WriteToken(securityToken);
        }
    }


}