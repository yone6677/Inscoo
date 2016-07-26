using System.ComponentModel;

namespace Models
{
    public class VHealthConfirmPayment
    {
        public string BaokuOrderCode { set; get; }
        public int MasterId { set; get; }
        [DisplayName("单价")]
        public decimal Price { get; set; }
        [DisplayName("购买份数")]
        public int Count { get; set; }

        [DisplayName("合计金额")]
        public decimal Amount { get; set; }
        [DisplayName("付款通知书")]
        public string PaymentNoticePdf { get; set; }
    }
}
