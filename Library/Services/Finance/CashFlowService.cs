using Core.Data;
using Domain.Finance;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
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
        public List<CashFlow> GetList(int type)
        {
            try
            {
                var query = _cashFlowRepository.Table;

                if (type > 0)
                {
                    query = query.Where(q => q.OType == type);
                }
                return query.ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowService：GetList");
            }
            return new List<CashFlow>();
        }
    }
}
