using Core.Pager;
using Domain;
using Models;

namespace Services
{
    public interface ICarInsuranceService
    {
        IPagedList<CarInsuranceDetail> GetDetails(CarInsuranceDetailSearchModel model, int pageIndex, int pageSize);
    }
}