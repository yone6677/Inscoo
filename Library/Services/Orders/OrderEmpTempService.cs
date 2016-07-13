using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Pager;
using Domain.Orders;
using Core.Data;
using Models.Infrastructure;
using Models.Order;

namespace Services.Orders
{
    public class OrderEmpTempService : IOrderEmpTempService
    {
        private readonly IRepository<OrderEmpTemp> _orderEmpTempRepository;
        private readonly ILoggerService _loggerService;
        private readonly IOrderBatchService _orderBatchService;
        public OrderEmpTempService(IRepository<OrderEmpTemp> orderEmpTempRepository, ILoggerService loggerService, IOrderBatchService orderBatchService)
        {
            _orderEmpTempRepository = orderEmpTempRepository;
            _loggerService = loggerService;
            _orderBatchService = orderBatchService;
        }

        public bool DeleteById(int id)
        {
            try
            {
                _orderEmpTempRepository.DeleteById(id);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpTempService：DeleteById");
                return false;
            }
        }
        public bool Delete(OrderEmpTemp item)
        {
            try
            {
                _orderEmpTempRepository.Delete(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpTempService：Delete");
                return false;
            }
        }
        public OrderEmpTemp GetById(int id)
        {
            try
            {
                return _orderEmpTempRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpTempService：DeleteById");
                return null;
            }
        }

        public OrderEmpTemp GetByInfo(string idNumber, string name, int bid)
        {
            try
            {
                var query = _orderEmpTempRepository.Table;
                if (bid > 0)
                {
                    query = query.Where(q => q.Bid == bid);
                }
                else
                {
                    return null;
                }
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(q => q.Name == name);
                }
                if (!string.IsNullOrEmpty(idNumber))
                {
                    query = query.Where(q => q.IDNumber == idNumber);
                }
                if (query.Any())
                {
                    return query.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetByInfo");

            }
            return null;
        }

        public List<OrderEmpTemp> GetListByBid(int bid)
        {
            try
            {
                var query = _orderEmpTempRepository.Table;
                query = query.Where(q => q.Bid == bid && q.IsDeleted == false);
                return query.ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetListByBid");
            }
            return new List<OrderEmpTemp>();
        }

        public IPagedList<OrderEmployeeModel> GetListOfPager(int pageIndex, int pageSize, int bid = 0)
        {
            try
            {
                var query = GetListByBid(bid);
                if (query.Count > 0)
                {
                    return new PagedList<OrderEmployeeModel>(query.Select(s => new OrderEmployeeModel
                    {
                        Id = s.Id,
                        BId = bid,
                        BuyType = s.BuyType,
                        BankCard = s.BankCard,
                        BankName = s.BankName,
                        Birthday = s.BirBirthday,
                        Email = s.Email,
                        EndDate = s.EndDate,
                        IDNumber = s.IDNumber,
                        IDType = s.IDType,
                        Name = s.Name,
                        PhoneNumber = s.PhoneNumber,
                        Premium = s.Premium,
                        Sex = s.Sex,
                        StartDate = s.StartDate,
                        HasSocialSecurity = s.HasSocialSecurity
                    }).ToList(), pageIndex, pageSize);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetListOfPager");
            }
            return new PagedList<OrderEmployeeModel>(new List<OrderEmployeeModel>(), pageIndex, pageSize);
        }

        public bool Insert(OrderEmpTemp item)
        {
            try
            {
                _orderEmpTempRepository.Insert(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpTempService：DeleteById");
                return false;
            }
        }
    }
}
