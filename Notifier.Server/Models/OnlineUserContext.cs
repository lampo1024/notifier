namespace Notifier.Server.Models
{
    public class OnlineUserContext
    {
        /// <summary>
        /// 在线用户列表
        /// </summary>
        public static List<ChatUser> OnlineChatUsers = new List<ChatUser>();
        public static void AddUser(ChatUser user)
        {
            ArgumentNullException.ThrowIfNull(user.IdentityId, nameof(user.IdentityId));
            var exist = GetUser(user.IdentityId);
            if (exist != null)
            {
                //RemoveUser(user.IdentityId);
                exist.ConnectionId = user.ConnectionId;
            }
            OnlineChatUsers.Add(user);
        }
        public static void RemoveUser(string userId)
        {
            ArgumentNullException.ThrowIfNull(userId, nameof(userId));
            var user = GetUser(userId);
            if (user == null)
            {
                return;
            }
            OnlineChatUsers.Remove(user);
        }
        public static ChatUser? GetUser(string userId)
        {
            return OnlineChatUsers.FirstOrDefault(x => x.IdentityId == userId);
        }
    }
}
