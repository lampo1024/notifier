using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Notifier.Server.Dto.RequestModels;
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
            var model = new ChatMessageModel
            {
                Data = data
            };
            // await _hubContext.Clients.All.SendAsync("receiveMessage", model);
            await NotificationHubManager.SendMessage(model);
            return Ok(new { success = true, message = "消息接收成功" });
        }

        [HttpPost]
        [Route("/api/message/custom")]
        public async Task<IActionResult> Custom(CustomRequestModel request)
        {
            ArgumentNullException.ThrowIfNull(request.EventName, nameof(request.EventName));
            var model = new MessageRequestModel
            {
                Data = request.Data
            };
            await _hubContext.Clients.All.SendAsync(request.EventName, model);
            return Ok(new { success = true, message = "消息接收成功" });
        }
    }
}
