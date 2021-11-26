using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Notifier.Server.Hubs
{
    public class NotificationHubManager
    {
        private static IHubContext<NotificationHub>? _notificationHub;
        /// <summary>
        /// 初始化集线器
        /// </summary>
        /// <param name="consumerHub"></param>
        public static void Init(IHubContext<NotificationHub> notificationHub)
        {
            _notificationHub = notificationHub;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task SendMessage(ChatMessageModel model)
        {
            if (string.IsNullOrEmpty(model.From))
            {
                return;
            }
            model.SetTargetToMe();
            Console.WriteLine($"接收到消息：{JsonConvert.SerializeObject(model)}");
            switch (model.Group)
            {
                case "private":
                    await _notificationHub.Clients.User(model.From).SendAsync("receiveMessage", model);
                    break;
                case "chat":
                    model.SetTargetToYou();
                    await _notificationHub.Clients.AllExcept(model.From).SendAsync("receiveMessage", model);
                    model.SetTargetToMe();
                    await _notificationHub.Clients.User(model.From).SendAsync("receiveMessage", model);
                    break;
                default:
                    await _notificationHub.Clients.All.SendAsync("receiveMessage", model);
                    break;
            }
        }
    }
}
