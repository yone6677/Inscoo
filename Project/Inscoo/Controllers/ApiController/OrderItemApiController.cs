using Models;
using Models.Api.Order;
using Services.Orders;
using System.Collections.Generic;
using System.Web.Http;

namespace Inscoo.Controllers
{
    public class OrderItemApiController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IOrderEmpService _orderEmpService;
        public OrderItemApiController(IOrderService orderService, IOrderItemService orderItemService, IOrderEmpService orderEmpService)
        {
            _orderService = orderService;
            _orderItemService = orderItemService;
            _orderEmpService = orderEmpService;
        }
        public List<OrderItemApi> GetOrderItem(int id)
        {
            var model = new List<OrderItemApi>();
            var order = _orderService.GetByBId(id);
            if (order.orderItem.Count > 0)
            {
                foreach (var i in order.orderItem)
                {
                    var item = new OrderItemApi()
                    {
                        CoverageSum = i.CoverageSum,
                        Id = i.Id,
                        Insurer = order.Insurer,
                        PayoutRatio = i.PayoutRatio,
                        PolicyNumber = order.PolicyNumber,
                        Price = i.Price,
                        ProdType = i.ProdType,
                        SafeguardName = i.SafeguardName
                    };
                    model.Add(item);
                }
            }
            return model;
        }
    }
}
