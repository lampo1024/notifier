namespace Notifier.Server.Models
{
    /// <summary>
    /// 登录用户模型
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// 登录名
        /// </summary>
        public string? LoginName { get; set; }
        /// <summary>
        /// 用户显示名称
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string? Password { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 最近一次登录时间
        /// </summary>
        public DateTime LastLoginAt { get; set; }
    }
}
