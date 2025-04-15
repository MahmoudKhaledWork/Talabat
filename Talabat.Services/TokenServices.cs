using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services;

namespace Talabat.Services
{
    public class TokenServices : ITokenServieces
    {
        private readonly IConfiguration configuration;

        public TokenServices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            // PayLoad :
            // 1. Private Claims [ User Definend ]

            // Claims : Is Proprties For User Like : Name , Password
            var AuthClaims = new List<Claim>()
            {
                 new Claim (ClaimTypes.GivenName , user.DisplayName),
                 new Claim (ClaimTypes.Email , user.Email),
            };
            // Roles 
            var UserRoles = await userManager.GetRolesAsync(user);
            foreach (var Role in UserRoles)
            {
                AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
            }
            // Key
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            //"ValidIssuer": "https://localhost:7060",
            //"ValidAudience": "MySecureKey",
            //"DurationInDays": "2"
            // Registerd Claims :
            var Token = new JwtSecurityToken
            (
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDays"])),
            claims: AuthClaims,
            signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(Token);

        }
    }
}
