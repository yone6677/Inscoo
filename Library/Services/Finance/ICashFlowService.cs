using Domain.Finance;
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
        List<CashFlow> GetList(int type = 0);
    }
}
