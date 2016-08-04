using System;
using System.Collections.Generic;
using Domain.Finance;
using Core.Data;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using System.Linq;
using Microsoft.AspNet.Identity;
using Core.Pager;
using Models.Finance;

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
                item.Author = _authenticationManager.User.Identity.Name;
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
        public IPagedList<CashFlowDetailsModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, int cId = 0)
        {
            try
            {
                var query = GetList(cId);
                if (query.Count > 0)
                {
                    return new PagedList<CashFlowDetailsModel>(query.Select(s => new CashFlowDetailsModel()
                    {
                        ActualCollected = s.ActualCollected,
                        Id = s.Id,
                        Payable = s.Payable,
                        RealPayment = s.RealPayment,
                        Receivable = s.Receivable,
                        CreateTime = s.CreateTime,
                        Memo = s.Memo,
                        TransferVoucher = s.TransferVoucher
                    }).ToList(), pageIndex, pageSize);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "CashFlowDetailsService：GetListOfPager");
            }
            return new PagedList<CashFlowDetailsModel>(new List<CashFlowDetailsModel>(), pageIndex, pageSize); ;
        }
    }
}
