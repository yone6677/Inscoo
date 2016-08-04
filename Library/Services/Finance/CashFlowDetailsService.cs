using System;
using System.Collections.Generic;
using Domain.Finance;
using Core.Data;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System.Linq;
using Microsoft.AspNet.Identity;

namespace Services.Finance
{
    public class CashFlowDetailsService : ICashFlowDetailsService
    {
        private readonly IRepository<CashFlowDetails> _cashFlowDetalsRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        public CashFlowDetailsService(IRepository<CashFlowDetails> cashFlowDetalsRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService)
        {
            _cashFlowDetalsRepository = cashFlowDetalsRepository;
            _authenticationManager = authenticationManager;
            _loggerService = loggerService;
        }
        public bool Delete(CashFlowDetails item)
        {
            try
            {
                _cashFlowDetalsRepository.Delete(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowDetailsService：Delete");
            }
            return false;
        }

        public CashFlowDetails GetById(int id)
        {
            try
            {
                return _cashFlowDetalsRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowDetailsService：Delete");
            }
            return null;
        }

        public List<CashFlowDetails> GetList(int cId)
        {
            try
            {
                var query = _cashFlowDetalsRepository.Table;
                if (cId > 0)
                {
                    query = query.Where(q => q.cId == cId);
                }
                return query.ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowDetailsService：Delete");
            }
            return new List<CashFlowDetails>();
        }

        public bool Insert(CashFlowDetails item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.GetUserId();
                _cashFlowDetalsRepository.Insert(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowDetailsService：Delete");
            }
            return false;
        }

        public bool Update(CashFlowDetails item)
        {
            try
            {
                _cashFlowDetalsRepository.Update(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowDetailsService：Delete");
            }
            return false;
        }
    }
}
