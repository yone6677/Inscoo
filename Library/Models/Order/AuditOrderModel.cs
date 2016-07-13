using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.Order
{
    public class AuditOrderModel : BaseViewModel
    {
        public string Role { get; set; }
        /// <summary>
        /// 哪家公司的产品
        /// </summary>
        public string Insurer { get; set; }
        /// <summary>
        /// 当前用户所在公司
        /// </summary>
        public string UserCompany { get; set; }
        public int OId { get; set; }
        public int State { get; set; }
        /// <summary>
        /// 审核类型1.inscoo/2.finance/3.Insurer
        /// </summary>
        public int rid { get; set; }
        [DisplayName("是否同意")]
        public bool InscooAudit { get; set; }
        [DisplayName("是否同意")]
        public bool FinanceAudit { get; set; }
        [DisplayName("订单金额")]
        public decimal Price { get; set; }
        [DisplayName("收款金额")]
        [Required]
        public decimal AmountCollected { get; set; }
        [DisplayName("收款日期")]
        [Required]
        public DateTime CollectionDate { get; set; }
        [DisplayName("转账流水号")]
        [Required]
        public string TransferVoucher { get; set; }
        [DisplayName("审核意见/备注")]
        [Required]
        public string FinanceMemo { get; set; }
        [DisplayName("是否同意")]
        public bool InsurerAudit { get; set; }
        [DisplayName("保单号码")]
        [Required]
        public string PolicyNumber { get; set; }
        [DisplayName("快递单号")]
        [Required]
        public string CourierNumber { get; set; }
        [DisplayName("审核意见/备注")]
        public string InsurerMemo { get; set; }
    }
}
