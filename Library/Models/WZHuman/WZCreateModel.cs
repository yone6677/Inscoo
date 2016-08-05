using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models
{
    public class WZCreateModel
    {
        [Required]
        [Remote("IsUserExist", "User", ErrorMessage = "邮箱已占用")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
        [Display(Name = "邮件")]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "公司名称")]
        public string CompanyName { get; set; }
    }
}
