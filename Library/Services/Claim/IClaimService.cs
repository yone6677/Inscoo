using Core.Pager;
using Domain;
using Models;
using System.Web;

namespace Services
{
    public interface IClaimService
    {
        IPagedList<vClaimManagementDetailList> GetClaimsDetailList(int pageIndex, int pageSize, vClaimManagementDetailListSearch model);
        IPagedList<ClaimFilesList> GetClaimFileList(ClaimFilesListSearchModel model, int pageIndex, int pageSize);
        vCompanyEdit GetClaimsDetailById(int id);

        int InsertClaimFileList(HttpPostedFileBase file, string author);
    }
}