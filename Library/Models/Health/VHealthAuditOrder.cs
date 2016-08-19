using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models
{
    public class VHealthAuditOrder
    {
        public int MasterId { set; get; }
        public string DateTicks { set; get; }
        [DisplayName("公司全称")]
        public string CompanyName { get; set; }
        [DisplayName("联系人")]
        public string Linkman { get; set; }
        [DisplayName("联系电话")]
        public string PhoneNumber { get; set; }
        [DisplayName("联系地址")]
        public string Address { get; set; }
        [DisplayName("价格")]
        public decimal Price { get; set; }

        [DisplayName("购买份数")]
        public int Count { get; set; }

        [DisplayName("合计金额")]
        public decimal Amount { get; set; }
        [DisplayName("下单时间")]
        public DateTime CreateTime { get; set; }
        [DisplayName("体检项目名称")]
        public string prodName { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        [DisplayName("下单人")]
        public string Author { set; get; }

        /// <summary>
        /// 服务有效期
        /// </summary>
        [DisplayName("服务有效期")]
        public string ServicePeriod { get; set; }
        [DisplayName("过期时间")]
        public DateTime Expire { get; set; }
        /// <summary>
        /// 人员上传批次
        /// </summary>
        public List<long> ticksGroup { get; set; }
        public List<VCheckProductDetail> product { get; set; }
        public bool IsInscooOperator { get; set; }
        public bool IsFinance { get; set; }
        /// <summary>
        /// 订单状态 7:付款确认 11:OP审核通过 14:OP审核未通过 17:确认收款
        /// </summary>
        public decimal State { get; set; }
        [DisplayName("订单号")]
        public string BaokuOrderCode { set; get; }
    }
}
