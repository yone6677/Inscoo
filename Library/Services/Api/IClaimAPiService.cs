using Domain.Claim;
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
    }
}
