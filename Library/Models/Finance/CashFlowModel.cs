using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Finance
{
    public class CashFlowModel : BaseViewModel
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        [DisplayName("订单号")]
        public int OId { get; set; }
        /// <summary>
        /// 订单类型 1：保险订单
        /// </summary>
        [DisplayName("订单类型")]
        public string OType { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        [DisplayName("订单金额")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 差额
        /// </summary>
        [DisplayName("差额")]
        public decimal Difference { get; set; }
        [DisplayName("时间")]
        public DateTime CreateDate { get; set; }
    }
}
