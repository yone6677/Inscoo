using System;
using System.ComponentModel;

namespace Models.Order
{
    public class OrderBatchModel : BaseViewModel
    {
        [DisplayName("批次编号")]
        public string BNum { get; set; }
        [DisplayName("状态")]
        public string BState { get; set; }
        public int State { get; set; }
        [DisplayName("人员资料")]
        public string EmpInfoFile { get; set; }
        [DisplayName("人员资料盖章PDF")]
        public string EmpInfoFileSeal { get; set; }
        [DisplayName("投保单盖章PDF")]
        public string PolicySeal { get; set; }
        [DisplayName("付款通知书")]
        public string PaymentNoticePDF { get; set; }
        [DisplayName("投保操作时间")]
        public DateTime PolicyHolderDate { get; set; }
        [DisplayName("Inscoo确认时间")]
        public DateTime InscooConfirmDate { get; set; }
        [DisplayName("财务收款金额")]
        public decimal AmountCollected { get; set; }
        [DisplayName("财务收款时间")]
        public DateTime CollectionDate { get; set; }
        [DisplayName("财务确认时间")]
        public DateTime FinanceDate { get; set; }
        [DisplayName("转账流水号")]
        public string TransferVoucher { get; set; }
        [DisplayName("财务审核意见")]
        public string FinanceMemo { get; set; }
        [DisplayName("保险公司确认时间")]
        public DateTime InsurerConfirmDate { get; set; }
        [DisplayName("快递单号")]
        public string CourierNumber { get; set; }
        [DisplayName("保险公司审核意见")]
        public string InsurerMemo { get; set; }
        [DisplayName("批次总额")]
        public decimal OrderAmount { get; set; }
    }
}
