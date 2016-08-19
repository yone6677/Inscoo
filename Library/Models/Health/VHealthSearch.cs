using System.ComponentModel;
using System.Web.Mvc;

namespace Models
{
    public class VHealthSearch
    {
        /// <summary>
        /// 列表类型 1客户未完成，2客户已完成，未审核，3已审核,4客户已完成
        /// </summary>
        public string ListType { set; get; }
        [DisplayName("订单号")]
        public string OrderNumber { get; set; }
        public SelectList ListTypeList { set; get; }
        [DisplayName("客户")]
        public string UserName { set; get; }
        [DisplayName("产品名称")]
        public string ProductName { set; get; }

        public bool IsInscooOperator { set; get; }
        public bool IsFinance { set; get; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
