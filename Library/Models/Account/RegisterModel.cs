using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Inscoo.Models.Account
{
    public class RegisterModel
    {
        [Display(Name = "角色")]
        public string Roles { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "企业名称")]
        public string CompanyName { get; set; }
        [Required]
        [Display(Name = "联系人")]
        public string Linkman { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "电话")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "返点分配")]
        [Range(0,0)]
        public int Rebate { set; get; }

        [Display(Name = "利润加成")]
        public bool TiYong { set; get; }
        [Display(Name = "理赔比率")]
        public bool FanBao { set; get; }
        [Display(Name = "返点（%）")]
        public int Rebate { get; set; }

        [Display(Name = "是否启用")]
        public bool IsDelete { set; get; }
        public List<SelectListItem> selectList { get; set; }
    }
}