using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notifier.Core;
using Notifier.Server.DataAccess;
using Notifier.Server.Dto.RequestModels.User;
using Notifier.Server.Dto.ResponseModels;
using Notifier.Server.Entities;
using Notifier.Server.Validations.User;
using System.Security.Claims;

namespace Notifier.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly NtfDbContext _db;

        public UserController(NtfDbContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _db.User.Where(x => !x.IsDeleted && x.IsActive)
                .ToListAsync();
            return Ok(users);
        }

        /// <summary>
        /// create user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel model)
        {
            var user = model.Adapt<NtfUser>();
            user.Code = CodeBuilder.CreateUserCode();
            user.IpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            user.IsActive = true;
            user.IsDeleted = false;
            user.CreatedAt = DateTime.Now;
            _db.User.Add(user);
            await _db.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// user login
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            if (loginModel.Username == null)
            {
                ArgumentNullException.ThrowIfNull(nameof(loginModel.Username));
                return Ok();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.Username)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return Ok();
        }

        /// <summary>
        /// user register
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel loginModel)
        {
            var response = ResponseModelFactory.GetResponseModel;
            var validator = new UserRegisterValidator();
            var result = validator.Validate(loginModel);
            if (!result.IsValid)
            {
                response.SetFailed("Register failed", result.Errors.Select(x => new
                {
                    x.PropertyName,
                    x.ErrorMessage,
                    x.AttemptedValue
                }));
                return Ok(response);
            }
            var user = loginModel.Adapt<NtfUser>();
            user.Code = CodeBuilder.CreateUserCode();
            user.IpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            user.IsActive = true;
            user.IsDeleted = false;
            user.CreatedAt = DateTime.Now;
            _db.User.Add(user);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
