using Core.Pager;
using Domain;
using Models;
using System.IO;

namespace Services
{
    public interface ICarInsuranceService
    {
        IPagedList<CarInsuranceDetail> GetDetails(CarInsuranceDetailSearchModel model, int pageIndex, int pageSize);
        byte[] DownLoadDetails(CarInsuranceDetailSearchModel model);
    }
}