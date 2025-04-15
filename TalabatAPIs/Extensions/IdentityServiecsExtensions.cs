using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Services;

namespace TalabatAPIs.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services , IConfiguration configuration)
        {
            // Add The Interfaces needed to the UserManager
            Services.AddIdentity<AppUser, IdentityRole>()
                            // The Class That Implement THe Interface 
                            .AddEntityFrameworkStores<AppIdentityDbContext>();
                            
            Services.AddScoped<ITokenServieces, TokenServices>();
            // Allow The Dependncy Injection To Identity
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                   //issuer , audience , expires , Key
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuer = true,
                            ValidIssuer = configuration["JWT:ValidIssuer"],
                            ValidateAudience = true,
                            ValidAudience = configuration["JWT:ValidAudience"],
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                        }; 
                    });
           
            return Services;
            #region Token 
            // Token : Is an Encrypted String
            // Saved in The User Local Storage

            // JWT : Json Web Token 
            // JWT Header  : 1 - Algorithm  | 2 - Type
            // JWT Payload : 1 - Register Claims  | 2 - Private Claims  

            #endregion

        }
    }
}
