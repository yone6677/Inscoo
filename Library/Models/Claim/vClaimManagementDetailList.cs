
using System;
using System.ComponentModel;
using System.Web.WebPages;

namespace Models
{
    public class vClaimManagementDetailList
    {
        public int Id { set; get; }
        [DisplayName("批次号")]
        public string ClaimBatch { get; set; }

        [DisplayName("团体名称")]
        public string InsuranceGroupName { get; set; }

        [DisplayName("保单号")]
        public string InsuranceNo { get; set; }

        [DisplayName("被保险人姓名")]
        public string InsuranceName { get; set; }
        [DisplayName("事故日期")]
        public DateTime ClaimAccdtDate { get; set; }
        [DisplayName("赔付日期")]
        public DateTime ClaimPayDate { get; set; }
        [DisplayName("费用总额")]
        public string ExpTotal { get; set; }
        [DisplayName("赔付金额")]
        public string ClaimAmt { get; set; }

    }
}
