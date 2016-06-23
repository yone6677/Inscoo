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
        List<OrderEmployee> GetList(int oid = 0);
        IPagedList<OrderEmployeeModel> GetListOfPager(int pageIndex, int pageSize, int oid = 0);
    }
}
