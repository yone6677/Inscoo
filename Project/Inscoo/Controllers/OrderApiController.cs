using Models.Api.Order;
using Services.Orders;
using System.Collections.Generic;
using System.Web.Http;

namespace Inscoo.Controllers
{
    public class OrderApiController : ApiController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderEmpService _orderEmpService;
        public OrderApiController(IOrderService orderService, IOrderItemService orderItemService, IOrderEmpService orderEmpService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _orderEmpService = orderEmpService;
        }
        [HttpPost]
        public List<OrderListApi> GetOrderList(GetOrderListApi query)
        {
            var model = new List<OrderListApi>();
            var emplist = _orderEmpService.GetByInfo(query.idNum, query.name);
            if (emplist.Count > 0)
            {
                foreach (var e in emplist)
                {
                    var order = _orderService.GetByBId(e.batch_Id);
                    var item = new OrderListApi()
                    {
                        OrderNum = order.OrderNum,
                        CompanyName = order.CompanyName,
                        EndDate = order.EndDate,
                        Id = order.Id,
                        StartDate = order.StartDate
                    };
                    if (!model.Contains(item))
                    {
                        model.Add(item);
                    }
                }
            }
            return model;
        }
    }
}
