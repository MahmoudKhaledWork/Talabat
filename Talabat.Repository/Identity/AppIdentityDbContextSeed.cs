using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Mahmoud Khaled",
                    Email = "MahmoudKhaled@gmail.com",
                    UserName = "MahmoudKhaled.Me",
                    PhoneNumber = "012345678910",
                };
                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
        }
    }
}
