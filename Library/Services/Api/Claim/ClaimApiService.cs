using Core.Data;
using Core.Pager;
using Domain.Claim;
using Models.Claim;
using Models.Infrastructure;
using Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Api
{
    public class ClaimApiService : IClaimAPiService
    {
        private readonly IRepository<ClaimFromWechatItem> _claimRepository;
        private readonly ILoggerService _loggerService;
        public ClaimApiService(IRepository<ClaimFromWechatItem> claimRepository, ILoggerService loggerService)
        {
            _claimRepository = claimRepository;
            _loggerService = loggerService;
        }
        public int insert(ClaimFromWechatItem model)
        {
            try
            {
                return _claimRepository.InsertGetId(model);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Error, "ClaimService：insert");
            }
            return 0;
        }
        public bool Update(ClaimFromWechatItem item)
        {
            try
            {
                _claimRepository.Update(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Error, "ClaimService：Update");
            }
            return false;
        }
        public ClaimFromWechatItem GetById(int id)
        {
            try
            {
                return _claimRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Error, "ClaimService：GetById");
            }
            return null;
        }
        public List<ClaimFromWechatItem> GetList(DateTime? beginDate = null, DateTime? endDate = null, int state = 0)
        {
            try
            {
                var query = _claimRepository.Table;
                query = query.Where(q => q.State == state);
                if (beginDate.HasValue)
                {
                    query = query.Where(q => q.CreateTime > beginDate.Value);
                }
                if (endDate.HasValue)
                {
                    query = query.Where(q => q.CreateTime < endDate.Value);
                }           
                return query.ToList();
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Error, "ClaimService：GetList");
            }
            return new List<ClaimFromWechatItem>();
        }
        public IPagedList<WechatClaimModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, DateTime? beginDate = null, DateTime? endDate = null, int state = 0)
        {
            try
            {
                var query = GetList(beginDate, endDate,state);
                if (query.Any())
                {
                    return new PagedList<WechatClaimModel>(query.Select(q => new WechatClaimModel()
                    {
                        Birthday = q.RecipientBirthday.ToShortDateString(),
                        Id = q.Id,
                        IdNumer = q.RecipientIdNumber,
                        Name = q.RecipientName,
                        PhoneNumber = q.RecipientPhone,
                        Sex = q.RecipientSex,
                        CreateTime = q.CreateTime

                    }).ToList(), pageIndex, pageSize);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Error, "ClaimService：GetListOfPager");
            }
            return new PagedList<WechatClaimModel>(new List<WechatClaimModel>(), pageIndex, pageSize);
        }
    }
}
