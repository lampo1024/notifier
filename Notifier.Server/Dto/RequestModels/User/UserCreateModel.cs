namespace Notifier.Server.Dto.RequestModels.User
{
    public class UserCreateModel
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
    }
}
