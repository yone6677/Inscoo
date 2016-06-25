using System;
using Domain.Orders;
using Core.Data;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System.Collections.Generic;
using Core.Pager;
using Models.Order;
using System.Linq;

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
                _loggerService.insert(e, LogLevel.Warning, "OrderService：Insert");

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
                _loggerService.insert(e, LogLevel.Warning, "OrderService：Update");
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
                _loggerService.insert(e, LogLevel.Warning, "OrderService：GetById");
            }
            return null;
        }
        public List<Order> GetList(string name = null, int state = 0, string companyName = null, DateTime? beginDate = null, DateTime? endDate = null)
        {
            try
            {
                var query = _orderRepository.Table;
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(q => q.Name.Contains(name));
                }
                if (state > 0)
                {
                    query = query.Where(q => q.State == state);
                }
                if (!string.IsNullOrEmpty(companyName))
                {
                    query = query.Where(q => q.CompanyName.Contains(companyName));
                }
                if (beginDate.HasValue)
                {
                    query = query.Where(q => q.CreateTime >= beginDate.Value);
                }
                if (endDate.HasValue)
                {
                    query = query.Where(q => q.CreateTime <= endDate.Value);
                }
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderService：GetList");
            }
            return new List<Order>();
        }
        public IPagedList<OrderListModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, string name = null, int state = 0, string companyName = null, DateTime? beginDate = null, DateTime? endDate = null)
        {
            try
            {
                var query = GetList(name, state, companyName, beginDate, endDate);
                if (query.Count > 0)
                {
                    return new PagedList<OrderListModel>(query.Select(s => new OrderListModel()
                    {
                        Amount = s.Amount,
                        AnnualExpense = s.AnnualExpense,
                        CompanyName = s.CompanyName,
                        CreateDate = s.CreateTime,
                        Id = s.Id,
                        InsuranceNumber = s.InsuranceNumber,
                        Name = s.Name,
                        StartDate = s.StartDate,
                        State = s.State
                    }).ToList(), pageIndex, pageSize);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderService：GetList");
            }
            return new PagedList<OrderListModel>(new List<OrderListModel>(), pageIndex, pageSize); ;
        }
    }
}
