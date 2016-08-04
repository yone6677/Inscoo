using Domain.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Finance
{
    public interface ICashFlowService
    {
        bool Delete(CashFlow item);
        bool Insert(CashFlow item);
        bool Update(CashFlow item);
        CashFlow GetById(int id);
        CashFlow GetByOid(int oId);
        List<CashFlow> GetList(int type = 0);
    }
}
