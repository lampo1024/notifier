namespace Notifier.Server.Models
{
    /// <summary>
    /// �����û�ģ��
    /// </summary>
    public class ChatUser
    {
        /// <summary>
        /// ��¼�û��ı�ʶID
        /// </summary>
        public string? IdentityId { get; set; }
        /// <summary>
        /// ʵʱ��������ID
        /// </summary>
        public string? ConnectionId { get; set; }
        /// <summary>
        /// �û�����
        /// </summary>
        public string? Name { get; set; }
    }
}