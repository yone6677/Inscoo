
namespace Models.Order
{
    public class CustomizeBuyModel
    {
        public string companyName { get; set; }
        public string productIds { get; set; }
        public string StaffsNum { get; set; }
        public string Avarage { get; set; }
        public string Price { get; set; }
        public int CustomizeProductId { get; set; }
        /// <summary>
        /// 方案名称 适用于推荐产品
        /// </summary>
        public string CaseName { get; set; }
        /// <summary>
        /// 年龄范围 适用于推荐产品
        /// </summary>
        public string AgeRangeName { get; set; }
    }
}