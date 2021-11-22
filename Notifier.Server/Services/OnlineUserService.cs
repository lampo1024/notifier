using Notifier.Server.Models;

namespace Notifier.Server.Services
{
    public class OnlineUserService
    {
        public OnlineUserService()
        {
            OnlineUserList = new List<ChatUser>();
        }
        public static List<ChatUser>? OnlineUserList { get; set; }
        public void AddUser(string connectionId, string name)
        {
            var user = new ChatUser
            {
                ConnectionId = connectionId,
                Name = name
            };
            OnlineUserList.Add(user);
        }

        public void RemoveUser(string connectionId)
        {
            var user = OnlineUserList.FirstOrDefault(x => x.ConnectionId == connectionId);
            if (user == null)
            {
                return;
            }
            OnlineUserList.Remove(user);
        }
    }
}