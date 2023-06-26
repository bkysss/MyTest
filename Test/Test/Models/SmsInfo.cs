using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
{
    [Table("sys_sms_info")]
    public class SmsInfo
    {
        [Key]
        [Column("sms_id")]
        public string smsId { get; set; }

        [Column("send_phone")]
        public string sendPhone { get; set; }

        [Column("sms_code")]
        public string smsCode { get; set; }

        [Column("is_valid")]
        public char? isValid { get; set; }

        [Column("expire_time")]
        public DateTime? expireTime { get; set; }

        [Column("create_user_id")]
        public string? createUserId { get; set; }

        [Column("create_time")]
        public DateTime? createTime { get; set; }

        [Column("last_mod_user_id")]
        public string? lastModUserId { get; set; }

        [Column("last_mod_time")]
        public DateTime? lastModTime { get; set; }
    }
}
