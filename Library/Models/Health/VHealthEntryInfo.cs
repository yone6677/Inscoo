using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models
{
    public class VHealthEntryInfo
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
        [Required(ErrorMessage = "请输入联系地址")]
        [Display(Name = "联系地址")]
        public string Address { get; set; }
        public decimal Status { get; set; }

        public int CompanyId { set; get; }
        public int MasterId { set; get; }
        public string DateTicks { set; get; }
        public string EmpInfoFileUrl { get; set; }

        [Display(Name = "返点信息")]
        public decimal CommentionRatio { get; set; }
        public SelectList CompanyNameList { set; get; }
    }
}
