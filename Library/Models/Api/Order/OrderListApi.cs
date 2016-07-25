using System;
namespace Models.Api.Order
{
    public class OrderListApi : BaseApiModel
    {
        /// <summary>
        /// 下单日期
        /// </summary>
        public DateTime CreateTime { set; get; }

        public DateTime EndDate { set; get; }

        public DateTime StartDate { set; get; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNum { get; set; }
        /// <summary>
        /// 保单号
        /// </summary>
        public string PolicyNumber { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
    }
}
