using Domain.Claim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Api
{
    public interface IClaimFileApiService
    {
        void insert(ClaimFileFromWechatItem item);
        List<ClaimFileFromWechatItem> GetByCId(int cId, int type = 0);
    }
}
