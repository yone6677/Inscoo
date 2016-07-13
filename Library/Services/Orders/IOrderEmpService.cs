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
        OrderEmployee GetById(int id);
        List<OrderEmployee> GetListByBid(int bid);
        List<OrderEmployee> GetListByOid(int oid);
        OrderEmployee GetByInfo(string idNumber, string name, int oid);

        IPagedList<OrderEmployeeModel> GetListOfPager(int pageIndex, int pageSize, int oid = 0);
        List<string> GetPdf(int oid);
        List<string> GetPaymentNoticePdf(int oid);
    }
}
