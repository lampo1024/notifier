using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json;

namespace Notifier.Server.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(MessageRequestModel model)
        {
            model.SetTargetToMe();
            Console.WriteLine($"接收到消息：{JsonConvert.SerializeObject(model)}");
            switch (model.Group)
            {
                case "private":
                    await Clients.Caller.SendAsync("receiveMessage", model);
                    break;
                case "chat":
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
            var uid = Context.GetHttpContext().Request.Query["uid"];
            if (string.IsNullOrEmpty(uid))
            {
                uid = "public";
            }

            Console.WriteLine($"用户[{uid}]连接[{Context.ConnectionId}]成功");
            await Groups.AddToGroupAsync(Context.ConnectionId, uid);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            var uid = Context.GetHttpContext().Request.Query["uid"];
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, uid);
            Console.WriteLine($"用户[{uid}]断开连接[{Context.ConnectionId}]");
            await base.OnDisconnectedAsync(ex);
        }
    }

    public record MessageRequestModel
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public string CreatedAsAstring => Created.ToString("yyyy-MM-dd HH:mm:ss.fff");
        /// <summary>
        /// 分组
        /// </summary>
        public string? Group { get; set; }
        public string? Type { get; set; } = "info";
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
