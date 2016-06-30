using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Orders;
using Core.Data;
using Microsoft.Owin.Security;
using Models.Infrastructure;

namespace Services.Orders
{
    public class OrderBatchService : IOrderBatchService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<OrderBatch> _orderBatchRepository;
        private readonly IAuthenticationManager _authenticationManager;
        public OrderBatchService(ILoggerService loggerService, IRepository<OrderBatch> orderBatchRepository, IAuthenticationManager authenticationManager)
        {
            _loggerService = loggerService;
            _orderBatchRepository = orderBatchRepository;
            _authenticationManager = authenticationManager;
        }
        public bool DeleteById(int id)
        {
            try
            {
                 _orderBatchRepository.DeleteById(id);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderBatch：DeleteById");
            }
            return false;
        }

        public OrderBatch GetById(int id)
        {
            try
            {
                return _orderBatchRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderBatch：GetById");
            }
            return null;
        }

        public OrderBatch GetByOrderId(int oid)
        {
            try
            {
                var list = GetList(oid);
                if (list.Count > 0)
                {
                    return list.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderBatch：GetByOrderId");
            }
            return null;
        }

        public bool Insert(OrderBatch item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _orderBatchRepository.Insert(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderBatch：Insert");
            }
            return false;
        }
        public int InsertGetId(OrderBatch item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
               return _orderBatchRepository.InsertGetId(item);
                
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderBatch：Insert");
            }
            return 0;
        }
        public bool Update(OrderBatch item)
        {
            try
            {
                _orderBatchRepository.Update(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderBatch：Insert");
            }
            return false;
        }
        public List<OrderBatch> GetList(int oid = 0)
        {
            try
            {
                var query = _orderBatchRepository.Table;
                if (oid > 0)
                {
                    query = query.Where(s => s.order_Id == oid);
                }
                return query.ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderBatch：GetList");
            }
            return new List<OrderBatch>();
        }
    }
}
