using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Models.Order
{
    public class OrderDatailsModel : BaseViewModel
    {
        public OrderDatailsModel()
        {
            orderBatch = new List<OrderBatchModel>();
            orderItem = new List<ProductModel>();
        }
        [DisplayName("方案名称")]
        public string Name { get; set; }
        [DisplayName("产品备注")]
        public string Memo { get; set; }
        [DisplayName("人数范围")]
        public string StaffRange { get; set; }
        [DisplayName("平均年龄")]
        public string AgeRange { get; set; }
        [DisplayName("费用(人/年)")]
        public decimal AnnualExpense { get; set; }
        [DisplayName("公司名称")]
        public string CompanyName { get; set; }
        [DisplayName("联系人")]
        public string Linkman { get; set; }
        [DisplayName("联系电话")]
        public string PhoneNumber { get; set; }
        [DisplayName("联系地址")]
        public string Address { get; set; }
        [DisplayName("营业执照")]
        public string BusinessLicense { get; set; }
        [DisplayName("保险公司")]
        public string Insurer { get; set; }
        [DisplayName("保单号码")]
        public string PolicyNumber { get; set; }
        [DisplayName("生效日期")]
        public string StartDate { get; set; }
        [DisplayName("失效日期")]
        public string EndDate { get; set; }
        [DisplayName("投保人数")]
        public int InsuranceNumber { get; set; }
        [DisplayName("订单状态")]
        public string State { get; set; }
        /// <summary>
        /// 当前用户角色
        /// </summary>
        public string Role { get; set; }
        public List<OrderBatchModel> orderBatch { get; set; }

        public List<ProductModel> orderItem { get; set; }
    }
}
