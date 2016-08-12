using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models.User
{
    public class EncryInfoModel
    {

        [DisplayName("邀请码")]
        public string AccountEncryCode { set; get; }

        [DisplayName("企业名称")]
        public string CompanyName { set; get; }

        [DisplayName("联系人")]
        public string LinkMan { set; get; }

        [Required]
        [DisplayName("邮箱")]
        [Remote("IsUserExist", "User")]
        [DataType(DataType.EmailAddress, ErrorMessage = "请输入正确的邮箱地址")]
        public string Email { set; get; }

        [DisplayName("电话")]
        public string Phone { set; get; }

        public string Password { set; get; }
        public string Mes { set; get; }

    }
}
