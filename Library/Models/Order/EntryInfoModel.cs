using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Order
{
   public class EntryInfoModel:BaseViewModel
    {
        [Required]
        [Display(Name = "公司全称")]
        public string CompanyName { get; set; }
        [Required]
        [Display(Name = "联系人")]
        public string Linkman { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "联系电话")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="保单及发票寄送需要联系地址")]
        [Display(Name = "联系地址")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "投保人数")]
        [Range(1,9999)]
        public int InsuranceNumber { get; set; }
        [Required]
        [Display(Name = "生效日期")]
        public DateTime StartDate { get; set; }
        [Required]
        [Range(0,9999 ,ErrorMessage ="还未上传人员资料")]
        public int IsUploadInfo { get; set; }
    }
}
