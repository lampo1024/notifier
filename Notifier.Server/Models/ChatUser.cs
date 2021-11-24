namespace Notifier.Server.Models
{
    /// <summary>
    /// 聊天用户模型
    /// </summary>
    public class ChatUser
    {
        /// <summary>
        /// 登录用户的标识ID
        /// </summary>
        public string? IdentityId { get; set; }
        /// <summary>
        /// 实时聊天连接ID
        /// </summary>
        public string? ConnectionId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string? Name { get; set; }
    }
}