using Core.Data;
using Domain.Orders;
using Microsoft.Owin.Security;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Orders
{
    public class OrderEmpService: IOrderEmpService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<OrderEmployee> _orderEmpRepository;
        private readonly IAuthenticationManager _authenticationManager;
        public OrderEmpService(ILoggerService loggerService, IRepository<OrderEmployee> orderEmpRepository, IAuthenticationManager authenticationManager)
        {
            _loggerService = loggerService;
            _orderEmpRepository = orderEmpRepository;
            _authenticationManager = authenticationManager;
        }
        public bool Insert(OrderEmployee item)
        {
            try
            {
                item.Author= _authenticationManager.User.Identity.Name;
                _orderEmpRepository.Insert(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Insert");
                return false;
            }
        }
    }
}
