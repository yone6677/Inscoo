using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.User
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "请输入原始密码")]
        [DataType(DataType.Password)]
        [DisplayName("原密码")]
        public string OldPassword { set; get; }

        [DisplayName("新密码")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "请输入新密码")]
        public string NewPassword { set; get; }

        [DisplayName("确认密码")]
        [Compare("NewPassword", ErrorMessage = "密码不一致")]
        [Required(ErrorMessage = "请输入确认密码")]
        [DataType(DataType.Password)]
        public string NewPasswordConfirm { set; get; }
    }
}
