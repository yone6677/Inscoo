using System.ComponentModel;

namespace Models.Order
{
    public class ConfirmPaymentModel : BaseViewModel
    {
        [DisplayName("保费单价")]
        public decimal YearPrice { get; set; }
        public double MonthPrice { get; set; }
        [DisplayName("购买数量")]
        public int Quantity { get; set; }
        [DisplayName("合计费用")]
        public decimal Amount { get; set; }
        [DisplayName("付款通知书")]
        public string PaymentNoticeUrl { get; set; }
    }
}
