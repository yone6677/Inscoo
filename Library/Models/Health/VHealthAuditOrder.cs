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

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
