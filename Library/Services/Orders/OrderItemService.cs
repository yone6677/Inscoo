using Core.Data;
using Domain.Orders;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System;

namespace Services.Orders
{
    public class OrderItemService : IOrderItemService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<OrderItem> _orderItemRepository;
        private readonly IAuthenticationManager _authenticationManager;
        public OrderItemService(ILoggerService loggerService, IRepository<OrderItem> orderItemRepository, IAuthenticationManager authenticationManager)
        {
            _loggerService = loggerService;
            _orderItemRepository = orderItemRepository;
            _authenticationManager = authenticationManager;
        }
        public bool Delete(OrderItem item)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(OrderItem item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _orderItemRepository.Insert(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderItem：Insert");
                return false;
            }

        }

        public bool Update(OrderItem item)
        {
            throw new NotImplementedException();
        }
    }
}
