using Core.Data;
using Core.Pager;
using Domain.Orders;
using Microsoft.Owin.Security;
using Models.Order;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Orders
{
    public class OrderEmpService : IOrderEmpService
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
        public bool DeleteById(int id)
        {
            try
            {
                _orderEmpRepository.DeleteById(id);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：DeleteById");
                return false;
            }
        }
        public bool Insert(OrderEmployee item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _orderEmpRepository.Insert(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Insert");
                return false;
            }
        }
        public List<OrderEmployee> GetList(int oid)
        {
            try
            {
                var query = _orderEmpRepository.Table;
                if (oid > 0)
                {
                    return query.Where(c => c.OId == oid).ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：GetList");
            }
            return null;
        }
        public IPagedList<OrderEmployeeModel> GetListOfPager(int pageIndex, int pageSize, int oid)
        {
            try
            {
                var query = GetList(oid);
                if (query.Any())
                {
                    return new PagedList<OrderEmployeeModel>(query.Select(s => new OrderEmployeeModel
                    {
                        Id = s.Id,
                        BankCard = s.BankCard,
                        BankName = s.BankName,
                        BirBirthday = s.BirBirthday,
                        Email = s.Email,
                        IDNumber = s.IDNumber,
                        IDType = s.IDType,
                        Name = s.Name,
                        PhoneNumber = s.PhoneNumber,
                        Premium = s.Premium,
                        Sex = s.Sex,
                        StartDate = s.StartDate,
                        HasSocialSecurity=s.HasSocialSecurity
                    }).ToList(), pageIndex, pageSize);

                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetListOfPager");
            }
            return new PagedList<OrderEmployeeModel>(new List<OrderEmployeeModel>(), pageIndex, pageSize);
        }
    }
}
