
namespace Domain.Products
{
   public class MixProductItem:BaseEntity
    {
        /// <summary>
        /// 混合产品ID
        /// </summary>
        public int mid { get; set; }
        /// <summary>
        /// 保障范围
        /// </summary>
        public string SafefuardName { get; set; }
        /// <summary>
        /// 项目原始价格
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 保障额度
        /// </summary>
        public string CoverageSum { get; set; }
        /// <summary>
        /// 赔付比率
        /// </summary>
        public string PayoutRatio { get; set; }
        /// <summary>
        /// 所属混合产品
        /// </summary>
        public virtual MixProduct prodMix { get; set; }
        /// <summary>
        /// 关联基础产品
        /// </summary>
        public virtual Product product { get; set; }
    }
}
