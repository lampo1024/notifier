using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notifier.Core;
using Notifier.Server.DataAccess;
using Notifier.Server.Dto.RequestModels.User;
using Notifier.Server.Entities;

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

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _db.User.Where(x => !x.IsDeleted && x.IsActive)
                .ToListAsync();
            return Ok(users);
        }

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
    }
}
