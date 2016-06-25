using Domain.Orders;
using System.Collections.Generic;

namespace Services.Orders
{
    public interface IOrderItemService
    {
        bool Insert(OrderItem item);
        bool Update(OrderItem item);
        bool Delete(OrderItem item);
        bool DeleteById(int id);
        List<OrderItem> GetList(int oid);
    }
}
