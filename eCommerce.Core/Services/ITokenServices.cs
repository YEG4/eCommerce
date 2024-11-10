using eCommerce.Core.Entities.Identity;

namespace eCommerce.Core.Services
{
    public interface ITokenServices
    {
        public Task<string> GetAccessToken(AppUser user);
    }
}