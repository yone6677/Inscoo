using Core.Data;
using Domain.Claim;
using Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Api
{
   public class ClaimFileApiService:IClaimFileApiService
    {
        private readonly IRepository<ClaimFileFromWechatItem> _claimFileApiRepository;
        private readonly ILoggerService _loggerService;
        public ClaimFileApiService(IRepository<ClaimFileFromWechatItem> claimFileApiRepository, ILoggerService loggerService)
        {
            _claimFileApiRepository = claimFileApiRepository;
            _loggerService = loggerService;
        }
        public void insert(ClaimFileFromWechatItem item)
        {
            try
            {
                _claimFileApiRepository.Insert(item);
            }
            catch(Exception e)
            {
                _loggerService.insert(e, LogLevel.Error, "ClaimFileApiService：insert");
            }
        }
    }
}
