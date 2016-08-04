using Core.Pager;
using Domain.Finance;
using Models.Finance;
using System;
using System.Collections.Generic;

namespace Services.Finance
{
    public interface ICashFlowService
    {
        bool Delete(CashFlow item);
        bool Insert(CashFlow item);
        int InsertGetId(CashFlow item);
        bool Update(CashFlow item);
        CashFlow GetById(int id);
        CashFlow GetByOid(int oId);
        List<CashFlow> GetList(int type = 0, int oId = 0, DateTime? beginDate = null, DateTime? endDate = null);
        IPagedList<CashFlowModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, int type = 0, int oId = 0, DateTime? beginDate = null, DateTime? endDate = null);
    }
}
