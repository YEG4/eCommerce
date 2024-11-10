using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eCommerce.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace eCommerce.Repository.Identity.DataSeed
{
    public static class IdentityDbContextSeed
    {
        public async static Task UsersSeedAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Ahmad Tarek",
                    Email = "admin@admin.com",
                    UserName = "admin",
                    PhoneNumber = "01020304050"
                };
                await userManager.CreateAsync(user, "Admin@123");
            }
        }
    }
}