using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Claim
{
    public class WechatClaimModel : BaseViewModel
    {

        [Display(Name = "姓名")]
        public string Name { get; set; }
        [Display(Name = "身份证号")]
        public string IdNumer { get; set; }
        [Display(Name = "性别")]
        public string Sex { get; set; }
        [Display(Name = "生日")]
        public string Birthday { get; set; }
        [Display(Name = "电话")]
        public string PhoneNumber { get; set; }
        [Display(Name = "申请日期")]
        public DateTime CreateTime { get; set; }
    }
}
