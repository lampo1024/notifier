namespace Notifier.Server.Dto.RequestModels.User
{
    public class UserRegisterModel
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string ConfirmPassword { get; set; } = "";
        public string? Avatar { get; set; }
        public string? DisplayName { get; set; }
    }
}