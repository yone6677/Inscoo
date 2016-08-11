using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class HomeIndexModel
    {
        //public HomeIndexModel() {
        //    InsuranceOrderCount = 0;
        //    InsurancePersonCount = 0;

        //}
        /// <summary>
        /// 有效保单数
        /// </summary>
        public int InsuranceOrderCount { set; get; }
        /// <summary>
        /// 在保人数量
        /// </summary>
        public int InsurancePersonCount { set; get; }
        /// <summary>
        /// 服务订单数量
        /// </summary>
        public int HealthOrderCount { set; get; }
        /// <summary>
        /// 服务总数量
        /// </summary>
        public int HealthPersonCount { set; get; }

    }
}
