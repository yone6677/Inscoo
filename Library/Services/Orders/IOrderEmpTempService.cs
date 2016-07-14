using Core.Pager;
using Domain.Orders;
using Models.Order;
using System.Collections.Generic;

namespace Services.Orders
{
    public interface IOrderEmpTempService
    {
        bool Insert(OrderEmpTemp item);
        bool DeleteById(int id);
        bool Delete(OrderEmpTemp item);
        OrderEmpTemp GetById(int id);
        List<OrderEmpTemp> GetListByBid(int bid);
        OrderEmpTemp GetByInfo(string idNumber, string name, int bid);
        IPagedList<OrderEmployeeModel> GetListOfPager(int pageIndex, int pageSize, int bid = 0);
    }
}
