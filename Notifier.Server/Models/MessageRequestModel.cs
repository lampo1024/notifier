namespace Notifier.Server.Hubs
{
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
