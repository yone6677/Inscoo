using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Orders;
using Core.Data;
using Microsoft.Owin.Security;
using Services.Infrastructure;

namespace Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<Order> _orderRepository;
        private readonly IAuthenticationManager _authenticationManager;
        public OrderService(ILoggerService loggerService, IRepository<Order> orderRepository, IAuthenticationManager authenticationManager)
        {
            _loggerService = loggerService;
            _orderRepository = orderRepository;
            _authenticationManager = authenticationManager;
        }
        public bool Delete(Order item)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(Order item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                item.Changer = item.Author;
                item.ChangeDate = DateTime.Now;
                item.State = 1;
                return _orderRepository.InsertGetId(item);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Insert");

            }
            return 0;
        }

        public bool Update(Order item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                item.Changer = item.Author;
                item.ChangeDate = DateTime.Now;
                _orderRepository.Update(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Update");
            }
            return false;
        }

        public Order GetById(int id)
        {
            try
            {
                return _orderRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Insert");
            }
            return null;
        }
    }
}
