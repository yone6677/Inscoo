using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.User
{
    public class UserModel
    {
        public string Id { get; set; }
        [DisplayName("名称")]
        public string Name { set; get; }
        [DisplayName("公司名称")]
        public string CompanyName { set; get; }
        [DisplayName("联系人")]
        public string LinkMan { set; get; }
        [DisplayName("利润加成")]
        public bool? TiYong { set; get; }
        [DisplayName("理赔比率")]
        public bool? FanBao { set; get; }
        [DisplayName("返点")]
        public int Rebate { get; set; }
        [DisplayName("邮箱")]
        public string Email { set; get; }
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
        [DisplayName("开户银行")]
        public string BankName { set; get; }
        [DisplayName("开户账号")]
        public string BankNumber { set; get; }
    }
}
