using Core.Pager;
using Domain.Finance;
using Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Finance
{
    public interface ICashFlowDetailsService
    {
        bool Delete(CashFlowDetails item);
        bool Insert(CashFlowDetails item);
        bool Update(CashFlowDetails item);
        CashFlowDetails GetById(int id);
        List<CashFlowDetails> GetList(int cId = 0);
        IPagedList<CashFlowDetailsModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, int cId = 0);
    }
}
