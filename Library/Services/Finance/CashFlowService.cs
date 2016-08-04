using Core.Data;
using Core.Pager;
using Domain.Finance;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models.Finance;
using Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Finance
{
    public class CashFlowService : ICashFlowService
    {
        private readonly IRepository<CashFlow> _cashFlowRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        public CashFlowService(IRepository<CashFlow> cashFlowRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService)
        {
            _cashFlowRepository = cashFlowRepository;
            _authenticationManager = authenticationManager;
            _loggerService = loggerService;
        }
        public bool Delete(CashFlow item)
        {
            try
            {
                _cashFlowRepository.Delete(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowService：Delete");
            }
            return false;
        }
        public bool Insert(CashFlow item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.GetUserId();
                item.Changer = item.Author;
                item.ChangeTime = DateTime.Now;
                _cashFlowRepository.Insert(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowService：Insert");
            }
            return false;
        }
        public int InsertGetId(CashFlow item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.GetUserId();
                item.Changer = item.Author;
                item.ChangeTime = DateTime.Now;
                return _cashFlowRepository.InsertGetId(item);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowService：InsertGetId");
            }
            return 0;
        }
        public bool Update(CashFlow item)
        {
            try
            {
                item.Changer = _authenticationManager.User.Identity.GetUserId();
                item.ChangeTime = DateTime.Now;
                _cashFlowRepository.Update(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowService：Update");
            }
            return false;
        }
        public CashFlow GetById(int id)
        {
            try
            {
                return _cashFlowRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowService：GetById");
            }
            return null;
        }
        public CashFlow GetByOid(int oId)
        {
            try
            {
                var query = _cashFlowRepository.Table;
                query = query.Where(q => q.OId == oId);
                if (query.Any())
                {
                    return query.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowService：GetByOid");
            }
            return null;
        }
        public List<CashFlow> GetList(int type = 0, int oId = 0, DateTime? beginDate = null, DateTime? endDate = null)
        {
            try
            {
                var query = _cashFlowRepository.Table;
                if (oId > 0)
                {
                    return query.Where(q => q.OId == oId).ToList();
                }
                if (type > 0)
                {
                    query = query.Where(q => q.OType == type);
                }
                if (beginDate.HasValue)
                {
                    query = query.Where(q => q.ChangeTime >= beginDate.Value);
                }
                if (endDate.HasValue)
                {
                    query = query.Where(q => q.ChangeTime <= endDate.Value);
                }

                return query.ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowService：GetList");
            }
            return new List<CashFlow>();
        }
        public IPagedList<CashFlowModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, int type = 0, int oId = 0, DateTime? beginDate = null, DateTime? endDate = null)
        {
            try
            {
                var query = GetList(type);
                if (query.Count > 0)
                {
                    return new PagedList<CashFlowModel>(query.Select(s => new CashFlowModel()
                    {
                        Amount = s.Amount,
                        CreateDate = s.CreateTime,
                        Difference = s.Difference,
                        Id = s.Id,
                        OId = s.OId,
                        OType = s.OType == 1 ? "保险订单" : s.OType.ToString()
                    }).ToList(), pageIndex, pageSize);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowService：GetListOfPager");
            }
            return new PagedList<CashFlowModel>(new List<CashFlowModel>(), pageIndex, pageSize); ;
        }
    }
}
