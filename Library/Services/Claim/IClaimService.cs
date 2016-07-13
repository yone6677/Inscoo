using Core.Pager;
using Models;

namespace Services
{
    public interface IClaimService
    {
        IPagedList<vClaimManagementDetailList> GetClaimsDetailList(int pageIndex, int pageSize, vClaimManagementDetailListSearch model);
        vCompanyEdit GetClaimsDetailById(int id);
    }
}