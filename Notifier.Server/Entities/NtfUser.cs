using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notifier.Server.Entities
{
    /// <summary>
    /// user entity
    /// </summary>
    [Table("ntf_user")]
    public class NtfUser
    {
        /// <summary>
        /// Auto-increment identity
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// User code
        /// </summary>
        [Required, StringLength(8)]
        public string Code { get; set; } = "";
        /// <summary>
        /// Login name
        /// </summary>
        [Required, StringLength(30)]
        public string LoginName { get; set; } = "";
        /// <summary>
        /// password
        /// </summary>
        [Required, StringLength(255)]
        public string Password { get; set; } = "";
        [StringLength(30)]
        public string FirstName { get; set; } = "";
        [StringLength(30)]
        public string LastName { get; set; } = "";
        [StringLength(30)]
        public string DisplayName { get; set; } = "";
        public short Gender { get; set; } = 0;
        [StringLength(255)]
        public string Avatar { get; set; } = "";
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// register ip address
        /// </summary>
        public string IpAddress { get; set; } = "";
        public DateTime? LastLoginAt { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
