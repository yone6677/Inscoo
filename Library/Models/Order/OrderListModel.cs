using System;
using System.ComponentModel;

namespace Models.Order
{
    public class OrderListModel : BaseViewModel
    {
        [DisplayName("方案名称")]
        public string Name { get; set; }
        [DisplayName("公司名称")]
        public string CompanyName { get; set; }
        [DisplayName("保费(人/年)")]
        public decimal AnnualExpense { get; set; }
        [DisplayName("人数")]
        public int InsuranceNumber { get; set; }
        [DisplayName("订单金额")]
        public decimal Amount { get; set; }
        [DisplayName("订单状态")]
        public string StateDesc { get; set; }
        public int State { get; set; }
        [DisplayName("生效日期")]
        public DateTime StartDate { get; set; }
        [DisplayName("失效日期")]
        public DateTime EndDate { get; set; }
        [DisplayName("创建日期")]
        public DateTime CreateDate { get; set; }
        public int BatchState { get; set; }       
    }
}
