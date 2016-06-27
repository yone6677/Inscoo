using System;
using System.ComponentModel;

namespace Models.Order
{
    public class OrderBatchModel : BaseViewModel
    {
        [DisplayName("批次编号")]
        public int BId { get; set; }
        [DisplayName("状态")]
        public string BState { get; set; }
        [DisplayName("人员资料")]
        public string EmpInfoFile { get; set; }
        [DisplayName("人员资料盖章PDF")]
        public string EmpInfoFileSeal { get; set; }
        [DisplayName("投保单盖章PDF")]
        public string PolicySeal { get; set; }
        [DisplayName("付款通知书")]
        public string PaymentNoticePDF { get; set; }
        [DisplayName("下单时间")]
        public string PolicyHolderDate { get; set; }
        [DisplayName("Inscoo确认时间")]
        public string InscooConfirmDate { get; set; }
        [DisplayName("收款金额")]
        public decimal AmountCollected { get; set; }
        [DisplayName("收款时间")]
        public string FinanceDate { get; set; }
        [DisplayName("保险公司确认时间")]
        public string InsurerConfirmDate { get; set; }
        [DisplayName("快递单号")]
        public string CourierNumber { get; set; }
    }
}
