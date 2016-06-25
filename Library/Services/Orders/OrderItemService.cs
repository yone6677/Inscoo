using Core.Data;
using Domain.Orders;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public List<OrderItem> GetList(int oid)
        {
            try
            {
                var query = _orderItemRepository.Table;
                query = query.Where(s => s.order_Id == oid);
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderItem：Insert");
            }
            return new List<OrderItem>();
        }
        public bool Update(OrderItem item)
        {
            throw new NotImplementedException();
        }
    }
}
