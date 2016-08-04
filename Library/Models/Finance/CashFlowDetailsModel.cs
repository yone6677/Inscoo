using System.ComponentModel;

namespace Models.Finance
{
    public class CashFlowDetailsModel : BaseViewModel
    {
        /// <summary>
        /// 应收款
        /// </summary>
        [DisplayName("应收款")]
        public decimal Receivable { get; set; }
        /// <summary>
        /// 实收款
        /// </summary>
        [DisplayName("实收款")]
        public decimal ActualCollected { get; set; }
        /// <summary>
        /// 应付款
        /// </summary>
        [DisplayName("应付款")]
        public decimal Payable { get; set; }
        /// <summary>
        /// 实付款
        /// </summary>
        [DisplayName("实付款")]
        public decimal RealPayment { get; set; }
    }
}
