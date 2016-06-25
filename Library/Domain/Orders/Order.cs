﻿
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
        /// 生效日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 营业执照
        /// </summary>
        public int BusinessLicense { get; set; }
        /// <summary>
        /// 意见书编号
        /// </summary>
        public string ProposalNo { get; set; }
        /// <summary>
        /// 保险公司
        /// </summary>
        public string Insurer { get; set; }
        /// <summary>
        /// 保险公司保单号
        /// </summary>
        public string PolicyNumber { get; set; }
        /// <summary>
        /// 保险公司确认日期
        /// </summary>
        public DateTime ConfirmedDate { get; set; }
        /// <summary>
        /// 投保人数
        /// </summary>
        public int InsuranceNumber { get; set; }
        /// <summary>
        /// 产品详细
        /// </summary>
        public virtual ICollection<OrderItem> orderItem { get; set; }
        /// <summary>
        /// 人员详细
        /// </summary>
        public virtual ICollection<OrderEmployee> orderEmp { get; set; }
        /// <summary>
        /// 投保批次
        /// </summary>
        public virtual OrderBatch orderBatch { get; set; }
    }
}
