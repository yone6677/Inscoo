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

        [MaxLength(50)]
        [Display(Name = "开户行")]
        public string BankName { get; set; }

        [MaxLength(50)]
        [DataType(DataType.CreditCard)]
        [Display(Name = "开户账号")]
        public string BankNumber { get; set; }

        [Display(Name = "返点分配")]
        public int Rebate { set; get; }

        [Display(Name = "利润加成")]
        public bool TiYong { set; get; }
        [Display(Name = "理赔比率")]
        public bool FanBao { set; get; }

        [Display(Name = "是否启用")]
        public bool IsDelete { set; get; }
        public List<SelectListItem> selectList { get; set; }
    }
}