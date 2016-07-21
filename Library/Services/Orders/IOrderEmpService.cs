using Core.Pager;
using Domain.Orders;
using Models.Order;
using System.Collections.Generic;

namespace Services.Orders
{
    public interface IOrderEmpService
    {
        bool Insert(OrderEmployee item);
        bool DeleteById(int id);
        bool Update(OrderEmployee item);
        /// <summary>
        /// 获取订单总额，减保人员也算在内
        /// </summary>
        /// <param name="oId"></param>
        /// <returns></returns>
        decimal GetOrderTotalAmount(int oId);
        OrderEmployee GetById(int id);
        List<OrderEmployee> GetListByBid(int bid);
        List<OrderEmployee> GetListByOid(int oid);
        List<OrderEmployee> GetByInfo(string idNumber, string name);
        OrderEmployee GetByInfo(string idNumber, string name, int oid);
        IPagedList<OrderEmployeeModel> GetListOfPager(int pageIndex, int pageSize, int oid = 0);
        List<string> GetPdfOfEmpTemp(int bid);
        List<string> GetPdf(int oid);
        List<string> GetPaymentNoticePdf(int oid, int bid = 0);
        /// <summary>
        /// 产生投保单
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        List<string> GetPolicyPdf(int oid);
    }
}
