using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.Finance
{
    public class CashFlowDetailsModel : BaseViewModel
    {
        public int cId { get; set; }
        /// <summary>
        /// 应收款
        /// </summary>
        [DisplayName("应收款")]
        public decimal Receivable { get; set; }
        /// <summary>
        /// 实收款
        /// </summary>
        [DisplayName("实收款")]
        [Required(ErrorMessage = "{0}必须填写")]
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
        [Required(ErrorMessage = "{0}必须填写")]
        public decimal RealPayment { get; set; }
        /// <summary>
        /// 转账凭证
        /// </summary>
        [DisplayName("转账凭证")]
        [Required(ErrorMessage = "{0}必须填写")]
        public string TransferVoucher { get; set; }

        [DisplayName("备注")]
        [Required(ErrorMessage = "{0}必须填写")]
        [MaxLength(500, ErrorMessage = "你输入的太多了")]
        public string Memo { get; set; }

        [DisplayName("操作人")]
        public string Author { get; set; }
        [DisplayName("操作时间")]
        public DateTime CreateTime { get; set; }
    }
}
