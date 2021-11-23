using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Notifier.Server.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(MessageRequestModel model)
        {
            Console.WriteLine($"connectionId:{Context.ConnectionId}");
            model.SetTargetToMe();
            Console.WriteLine($"接收到消息：{JsonConvert.SerializeObject(model)}");
            //var model = new { created = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), type = "chat", group = "chat", data };
            switch (model.Group)
            {
                case "private":
                    await Clients.Caller.SendAsync("receiveMessage", model);
                    break;
                case "chat":
                    //await Clients.All.SendAsync("receiveMessage", model);
                    model.SetTargetToYou();
                    await Clients.AllExcept(Context.ConnectionId).SendAsync("receiveMessage", model);
                    model.SetTargetToMe();
                    await Clients.Caller.SendAsync("receiveMessage", model);
                    break;
                default:
                    await Clients.All.SendAsync("receiveMessage", model);
                    break;
            }

        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "rector");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            //ArgumentNullException.ThrowIfNull(ex);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "rector");
            await base.OnDisconnectedAsync(ex);
        }
    }

    public record MessageRequestModel
    {
        public MessageRequestModel()
        {
            Created = DateTime.Now;
        }
        public DateTime Created { get; set; }
        public string CreatedAsAstring => Created.ToString("yyyy-MM-dd HH:mm:ss.fff");
        /// <summary>
        /// 分组
        /// </summary>
        public string? Group { get; set; }
        public string? Type { get; set; }
        public string? Target { get; set; }
        public dynamic? Data { get; set; }

        public void SetTargetToYou()
        {
            Target = "you";
        }

        public void SetTargetToMe()
        {
            Target = "me";
        }
    }
}
