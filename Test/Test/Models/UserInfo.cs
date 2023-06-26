using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
{
    [Table("sys_user_info")]
    public class UserInfo
    {
        [Key]
        [Column("user_id")]
        public string userId { get; set; }

        [Column("user_name")]
        [RegularExpression(@"^[\u4e00-\u9fa5]{2,50}")]
        public string userName { get; set; }

        [Column("user_idcard")]
        [RegularExpression(@"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$")]
        public string userIdCard { get; set; }

        [Column("user_phone")]
        [RegularExpression(@"^1[3456789]\d{9}$")]
        public string? userPhone { get; set; }

        [Column("user_type")]
        public string? userType { get; set; }

        [Column("user_birthday")]
        public DateTime? userBirthday { get; set; }

        [Column("user_gender")]
        public char? userGender { get; set; }

        [Column("register_time")]
        public DateTime? registerTime { get; set; }

        [Column("last_login_time")]
        public DateTime? lastLoginTime { get; set; }

        [Column("user_status")]
        public string? userStatus { get; set; }

        [Column("exception_status")]
        public string? exceptionStatus { get; set; }

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
