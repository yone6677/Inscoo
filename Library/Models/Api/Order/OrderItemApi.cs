
using System.ComponentModel;

namespace Models.Api.Order
{
    public class OrderItemApi : BaseApiModel
    {
        /// <summary>
        /// 保险公司
        /// </summary>
        [DisplayName("保险公司")]
        public string Insurer { get; set; }
        /// <summary>
        /// 保单号码
        /// </summary>
        [DisplayName("保单号码")]
        public string PolicyNumber { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [DisplayName("类别")]
        public string ProdType { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("名称")]
        public string SafeguardName { set; get; }
        /// <summary>
        /// 保额
        /// </summary>
        [DisplayName("保额")]
        public string CoverageSum { set; get; }
        /// <summary>
        /// 赔付比率
        /// </summary>
        [DisplayName("赔付比例")]
        public string PayoutRatio { set; get; }
        /// <summary>
        /// 售价
        /// </summary>
        [DisplayName("售价")]
        public decimal Price { get; set; }
    }
}
