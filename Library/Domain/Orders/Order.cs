
using System;
using System.Collections.Generic;

namespace Domain.Orders
{
    public class Order : BaseEntity
    {
        /// <summary>
        /// 订单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 产品备注
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 投保人数
        /// </summary>
        public string StaffRange { get; set; }
        /// <summary>
        /// 平均年龄
        /// </summary>
        public string AgeRange { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        public decimal AnnualExpense { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 对外售价
        /// </summary>
        public decimal Pretium { get; set; }
        /// <summary>
        /// 利润加成
        /// </summary>
        public int TiYong { set; get; }
        /// <summary>
        /// 理赔比率
        /// </summary>
        public int FanBao { set; get; }
        /// <summary>
        /// 返点
        /// </summary>
        public int Rebate { get; set; }
        /// <summary>
        /// 佣金计算方法
        /// </summary>
        public string CommissionType { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 改动者
        /// </summary>
        public string Changer { get; set; }
        public DateTime ChangeDate { get; set; }
        /// <summary>
        /// 产品详细
        /// </summary>
        public virtual ICollection<OrderItem> orderItem { get; set; }
    }
}
