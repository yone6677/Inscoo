using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Finance
{
    public class CashFlow : BaseEntity
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public int OId { get; set; }
        /// <summary>
        /// 订单类型 1：保险订单
        /// </summary>
        public int OType { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 差额
        /// </summary>
        public decimal Difference { get; set; }
        /// <summary>
        /// 账目明细
        /// </summary>
        public ICollection<CashFlowDetails> cashFlowDetails { get; set; }

        public DateTime ChangeTime { get; set; }

        public string Changer { get; set; }
    }
}
