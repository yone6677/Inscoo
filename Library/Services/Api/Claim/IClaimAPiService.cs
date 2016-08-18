using Core.Pager;
using Domain.Claim;
using Models.Claim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Api
{
    public interface IClaimAPiService
    {
        int insert(ClaimFromWechatItem model);
        ClaimFromWechatItem GetById(int id);
        bool Update(ClaimFromWechatItem item);
        List<ClaimFromWechatItem> GetList(DateTime? beginDate = null, DateTime? endDate = null, int state = 0);
        IPagedList<WechatClaimModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, DateTime? beginDate = null, DateTime? endDate = null, int state = 0);
    }
}
