using System.Threading.Tasks;
using Domain;
using Core.Pager;
using Models;

namespace Services
{
    public interface IWZHumanService
    {
        Task AddNewWZHum2anMaster(WZHumanMaster model);
        /// <summary>
        /// 获取吴中客户列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<WZListModel> GetWZList(WZSearchModel search, int pageIndex = 1, int pageSize = 15);
    }
}