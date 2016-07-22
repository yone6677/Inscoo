
namespace Domain.Orders
{
   public class OrderItem: BaseEntity
    {
        public int pid { get; set; }
        /// <summary>
        /// 产品类别
        /// </summary>
        public string ProdType { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        public string SafeguardName { set; get; }
        /// <summary>
        /// 产品代码
        /// </summary>
        public string SafeguardCode { set; get; }
        /// <summary>
        /// 被保险人类型
        /// </summary>
        public string InsuredWho { set; get; }
        /// <summary>
        /// 保额
        /// </summary>
        public string CoverageSum { set; get; }
        /// <summary>
        /// 赔付比率
        /// </summary>
        public string PayoutRatio { set; get; }
        /// <summary>
        /// 售价.平均年龄导致售价迥异
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 佣金比率
        /// </summary>
        public float CommissionRate { get; set; }

        public virtual Order order { get; set; }
        public int order_Id { get; set; }
    }
}
