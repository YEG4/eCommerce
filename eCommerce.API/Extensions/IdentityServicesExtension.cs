using eCommerce.Core.Entities.Identity;
using eCommerce.Repository.Identity;
using Microsoft.AspNetCore.Identity;

namespace eCommerce.API.Extensions
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services)
        {
            Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppIdentityDbContext>();
            Services.AddAuthentication();
            return Services;
        }
    }
}