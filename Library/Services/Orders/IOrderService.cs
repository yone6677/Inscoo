using Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Orders
{
    public interface IOrderService
    {
        int Insert(Order item);
        bool Update(Order item);
        bool Delete(Order item);
        bool DeleteById(int id);
        Order GetById(int id);
    }
}
