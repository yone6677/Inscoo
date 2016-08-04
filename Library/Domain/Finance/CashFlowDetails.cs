using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Finance
{
    public class CashFlowDetails : BaseEntity
    {
        /// <summary>
        /// 应收款
        /// </summary>
        public decimal Receivable { get; set; }
        /// <summary>
        /// 实收款
        /// </summary>
        public decimal ActualCollected { get; set; }
        /// <summary>
        /// 应付款
        /// </summary>
        public decimal Payable { get; set; }
        /// <summary>
        /// 实付款
        /// </summary>
        public decimal RealPayment { get; set; }
        public CashFlow cashFlow { get; set; }
        /// <summary>
        /// 转账凭证
        /// </summary>
        public string TransferVoucher { get; set; }
        public string Memo { get; set; }
        public int cId { get; set; }
    }
}
