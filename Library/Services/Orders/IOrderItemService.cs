using Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Orders
{
    public interface IOrderItemService
    {
        bool Insert(OrderItem item);
        bool Update(OrderItem item);
        bool Delete(OrderItem item);
        bool DeleteById(int id);
    }
}
