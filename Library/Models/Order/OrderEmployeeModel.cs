using System;
using System.ComponentModel;

namespace Models.Order
{
    public class OrderEmployeeModel : BaseViewModel
    {
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("证件类型")]
        public string IDType { get; set; }
        [DisplayName("证件号码")]
        public string IDNumber { get; set; }
        [DisplayName("生日")]
        public DateTime BirBirthday { get; set; }
        [DisplayName("性别")]
        public string Sex { get; set; }
        [DisplayName("费用")]
        public decimal Premium { get; set; }
        [DisplayName("银行账号")]
        public string BankCard { get; set; }
        [DisplayName("开户行")]
        public string BankName { get; set; }
        [DisplayName("联系电话")]
        public string PhoneNumber { get; set; }
        [DisplayName("邮箱")]
        public string Email { get; set; }
        [DisplayName("是否有社保")]
        public string HasSocialSecurity { get; set; }
        [DisplayName("生效日期")]
        public DateTime StartDate { get; set; }
        [DisplayName("结束日期")]
        public DateTime EndDate { get; set; }
    }
}
