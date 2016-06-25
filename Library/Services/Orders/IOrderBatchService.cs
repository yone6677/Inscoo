using Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Orders
{
    public interface IOrderBatchService
    {
        bool Insert(OrderBatch item);
        bool DeleteById(int id);
        bool Update(OrderBatch item);
        OrderBatch GetById(int id);
        OrderBatch GetByOrderId(int oid);
        List<OrderBatch> GetList(int oid = 0);
    }
}
