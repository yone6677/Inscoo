using Core.Data;
using Domain.Claim;
using Models.Infrastructure;
using Services.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Api
{
    public class ClaimApiService: IClaimAPiService
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
            catch(Exception e)
            {
                _loggerService.insert(e, LogLevel.Error, "ClaimService：insert");
            }
            return 0;
        }
    }
}
