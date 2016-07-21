using Core.Pager;
using Domain.Orders;
using Models.Order;
using System;
using System.Collections.Generic;

namespace Services.Orders
{
    public interface IOrderService
    {
        int Insert(Order item);
        bool Update(Order item);
        bool Delete(Order item);
        bool DeleteById(int id);
        Order GetById(int id);
        Order GetByBId(int bid);
        List<Order> GetList(string name = null, int state = 0, string companyName = null, DateTime? beginDate = null, DateTime? endDate = null);
        IPagedList<OrderListModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, string name = null, int state = 0, string companyName = null, DateTime? beginDate = null, DateTime? endDate = null);
    }
}
