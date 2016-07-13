
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages;

namespace Models
{
    public class vClaimManagementDetailListSearch
    {

        [DisplayName("团体名称")]
        public string InsuranceGroupName { get; set; }

        [DisplayName("保单号")]
        public string InsuranceNo { get; set; }

        [DisplayName("被保险人姓名")]
        public string InsuranceName { get; set; }

        [DisplayName("事故日期开始")]
        [DataType(DataType.DateTime)]
        public DateTime ClaimAccdtDateBegin { get; set; }
        [DisplayName("事故日期结束")]
        [DataType(DataType.DateTime)]
        public DateTime ClaimAccdtDateEnd { get; set; }
    }
}
