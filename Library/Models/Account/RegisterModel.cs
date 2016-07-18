using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models
{
    public class RegisterModel
    {
        public string Id { set; get; }
        [Display(Name = "角色")]
        public string Roles { get; set; }

        //[Remote("IsUserExist", "User")]
        //[Display(Name = "登陆用户名")]
        public string UserName { get; set; }

        [Display(Name = "企业名称")]
        [MaxLength(120, ErrorMessage = "请检查输入")]
        public string CompanyName { get; set; }

        [Required]
        [Display(Name = "联系人")]
        public string Linkman { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "电话")]
        public string PhoneNumber { get; set; }
        [Required]
        [Remote("IsUserExist", "User")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }

        [MaxLength(50)]
        [Display(Name = "开户行")]
        public string BankName { get; set; }

        [Display(Name = "户名")]
        public string AccountName { get; set; }

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

        [Display(Name = "是否停用")]
        public bool IsDelete { set; get; }
        public SelectList RoleSelects { get; set; }
        public SelectList CommissionMethods { get; set; }

        [Display(Name = "自选产品权限(保险公司)")]
        public string[] ProdInsurances { get; set; }

        [Display(Name = "专属产品权限")]
        public string[] ProdSeries { get; set; }

        [Display(Name = "佣金计算方法")]
        public string CommissionMethod { get; set; }
    }
}