using Microsoft.AspNetCore.SignalR;

using Newtonsoft.Json;

using Notifier.Server.Models;

namespace Notifier.Server.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly Group _group = new Group();
        public async Task SendMessage(ChatMessageModel model)
        {
            model.From = Context.ConnectionId;
            //await NotificationHubManager.SendMessage(model);
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

        //public async Task Notify(MessageRequestModel model)
        //{
        //    Console.WriteLine($"接收到消息[onNotify]：{JsonConvert.SerializeObject(model)}");
        //    await Clients.All.SendAsync("onNotify", model);
        //}

        //public async Task Tooltip(MessageRequestModel model)
        //{
        //    Console.WriteLine($"接收到消息[onTooltip]：{JsonConvert.SerializeObject(model)}");
        //    await Clients.All.SendAsync("onTooltip", model);
        //}

        public override async Task OnConnectedAsync()
        {
            var uid = Context.GetHttpContext().Request.Query["uid"];
            if (string.IsNullOrEmpty(uid))
            {
                uid = "public";
            }
            var user = new ChatUser
            {
                IdentityId = uid,
                ConnectionId = Context.ConnectionId,
                Name = uid
            };
            OnlineUserContext.AddUser(user);
            Console.WriteLine($"用户[{uid}]连接[{user.ConnectionId}]成功");
            await Groups.AddToGroupAsync(user.ConnectionId, _group.Id);
            await base.OnConnectedAsync();
            //await Clients.All.SendAsync("refreshUserList", OnlineUserContext.OnlineChatUsers);

            RefreshUserList(uid);
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            var uid = Context.GetHttpContext().Request.Query["uid"];
            OnlineUserContext.RemoveUser(uid);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _group.Id);
            Console.WriteLine($"用户[{uid}]断开连接[{Context.ConnectionId}]");
            await base.OnDisconnectedAsync(ex);
            RefreshUserList(uid);
        }

        /// <summary>
        /// 刷新用户列表
        /// </summary>
        private async void RefreshUserList(string currentUserId)
        {
            Console.WriteLine($"refresh user:{currentUserId}--{Context.ConnectionId}");
            Console.WriteLine($"当前在线用户列表：{JsonConvert.SerializeObject(OnlineUserContext.OnlineChatUsers)}");
            var onlineUsers = OnlineUserContext.OnlineChatUsers
                .Select(x => new
                {
                    x.Name,
                    x.IdentityId,
                    x.ConnectionId,
                    me = x.ConnectionId == Context.ConnectionId
                })
                .OrderBy(x => x.Name);
            Console.WriteLine($"当前在线用户列表(After)：{JsonConvert.SerializeObject(onlineUsers)}");
            await Clients.Group(_group.Id).SendAsync("refreshUserList", onlineUsers);
            //await Clients.Caller.SendAsync("refreshUserList", onlineUsers);
        }
    }
}
