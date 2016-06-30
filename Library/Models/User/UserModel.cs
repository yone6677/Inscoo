using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models.User
{
    public class UserModel
    {
        public string Id { get; set; }

        [DisplayName("名称")]
        public string Name { set; get; }

        [Required]
        [DisplayName("公司名称")]
        public string CompanyName { set; get; }

        [Required]
        [DisplayName("联系人")]
        public string LinkMan { set; get; }

      
        [DisplayName("利润加成")]
        public bool? TiYong { set; get; }

       
        [DisplayName("理赔比率")]
        public bool? FanBao { set; get; }


        [DisplayName("返点")]
        public int Rebate { get; set; }

        [Required]
        [DisplayName("邮箱")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
        public string Email { set; get; }

        [Required]
        [DisplayName("电话")]
        public string Phone { set; get; }

        [DisplayName("角色")]
        public string RoleName { set; get; }
        [DisplayName("权限")]
        public List<string> RoleIds { set; get; }


        [DisplayName("注册时间")]
        public DateTime CreateTime { set; get; }
        [DisplayName("注册人")]
        public string CreaterId { set; get; }

        [Display(Name = "户名")]
        public string AccountName { get; set; }
        [MaxLength(50)]
        [DisplayName("开户银行")]
        public string BankName { set; get; }


        [MaxLength(50)]
        [DisplayName("开户账号")]
        public string BankNumber { set; get; }

        public SelectList CommissionMethods { get; set; }
        [DisplayName("佣金计算方法")]
        public string CommissionMethod { get; set; }
    }
}
