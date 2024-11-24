using eCommerce.API.DTOs;
using eCommerce.API.Errors;
using eCommerce.Core.Entities.Identity;
using eCommerce.Core.JsonObjects;
using eCommerce.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace eCommerce.API.Controllers
{
    public class UsersController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public JwtOptions jwtOptions { get; }
        private readonly ITokenServices tokenServices;
        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, JwtOptions jwtOptions, ITokenServices tokenServices)
        {
            this.tokenServices = tokenServices;
            this.jwtOptions = jwtOptions;
            _signInManager = signInManager;
            _userManager = userManager;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            var user = new AppUser
            {
                Email = model.Email,
                DisplayName = model.DisplayName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.Email.Split('@')[0]
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiErrorResponse(400));

            return Ok(new UserDTO
            {
                Email = model.Email,
                DisplayName = model.DisplayName,
                Token = await tokenServices.GetAccessToken(user)
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            _userManager.FindByEmailAsync()
            if (user is null) return Unauthorized(new ApiErrorResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return Unauthorized(new ApiErrorResponse(401));
            return Ok(new UserDTO
            {
                Email = model.Email,
                DisplayName = user.DisplayName,
                Token = await tokenServices.GetAccessToken(user)
            });

        }
    }
}