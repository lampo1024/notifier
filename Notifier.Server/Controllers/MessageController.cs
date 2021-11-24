using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using Notifier.Server.Hubs;

namespace Notifier.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public MessageController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet, HttpPost]
        [Route("/api/message/push")]
        public async Task<IActionResult> Push([FromBody] object data)
        {
            var model = new MessageRequestModel
            {
                Data = data
            };
            await _hubContext.Clients.All.SendAsync("receiveMessage", model);
            return Ok(new { success = true, message = "消息接收成功" });
        }
    }
}
