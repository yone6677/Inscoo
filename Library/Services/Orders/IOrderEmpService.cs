using Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Orders
{
   public interface IOrderEmpService
    {
        bool Insert(OrderEmployee item);
    }
}
