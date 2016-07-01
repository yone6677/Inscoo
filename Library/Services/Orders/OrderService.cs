using System;
using Domain.Orders;
using Core.Data;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System.Collections.Generic;
using Core.Pager;
using Models.Order;
using System.Linq;
using Services;

namespace Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<Order> _orderRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IAppUserService _appUserService;
        private readonly IAppRoleService _appRoleService;
        private readonly IGenericAttributeService _genericAttributeService;
        public OrderService(ILoggerService loggerService, IRepository<Order> orderRepository, IAuthenticationManager authenticationManager, IAppUserService appUserService,
            IGenericAttributeService genericAttributeService, IAppRoleService appRoleService)
        {
            _loggerService = loggerService;
            _orderRepository = orderRepository;
            _authenticationManager = authenticationManager;
            _appUserService = appUserService;
            _genericAttributeService = genericAttributeService;
            _appRoleService = appRoleService;
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
                //根据角色获得列表
                var user = _appUserService.GetCurrentUser();
                var role = _appRoleService.FindByIdAsync(user.Roles.FirstOrDefault().RoleId).Name;
                if (role == "PartnerChannel" || role == "CompanyHR")
                {
                    query = query.Where(q => q.Author == user.UserName);//这两种角色只能查看自己的订单
                }
                query = query.Where(q => q.IsDeleted == false);
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(q => q.Name.Contains(name));
                }
                if (state == 0)
                {
                    query = query.Where(q => q.State > 3);//默认返回已输完信息的所有订单
                }
                else if (state == 10)
                {
                    query = query.Where(q => q.State <= 5);//信息缺失的订单
                }
                else
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
                        Amount = GetPrice(s),//s.AnnualExpense * s.InsuranceNumber,
                        AnnualExpense = s.AnnualExpense,
                        CompanyName = s.CompanyName,
                        CreateDate = s.CreateTime,
                        Id = s.Id,
                        InsuranceNumber = s.InsuranceNumber,
                        Name = s.Name,
                        StartDate = s.StartDate,
                        StateDesc = _genericAttributeService.GetByKey(null, "orderState", s.State.ToString()).Key,
                        State = s.State,
                        BatchState=s.orderBatch.Where(b=>b.InsurerConfirmDate==DateTime.MinValue).Any()
                    }).OrderByDescending(s => s.CreateDate).ToList(), pageIndex, pageSize);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderService：GetList");
            }
            return new PagedList<OrderListModel>(new List<OrderListModel>(), pageIndex, pageSize); ;
        }
        public decimal GetPrice(Order item)
        {
            try
            {
                decimal total = 0;
                var batch = item.orderBatch.Where(b => b.IsDeleted == false);
                if (batch.Any())
                {
                   foreach(var b in batch)
                    {
                        if (b.orderEmp.Any())
                        {
                            total += b.orderEmp.Sum(e => e.Premium);
                        }
                    }
                }
                return total;
            }
            catch(Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderService：GetPrice");
            }
            return 0;
        }
    }
}
