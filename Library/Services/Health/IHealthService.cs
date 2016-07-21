using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Core.Pager;
using Domain;
using Models;
using Models.Order;

namespace Services
{
    public interface IHealthService
    {
        /// <summary>
        /// ����ȷ�ϣ������ݿ����һ����¼
        /// </summary>
        /// <param name="productId">��Ʒ</param>
        /// <param name="author">������</param>
        /// <returns></returns>
        Task AddHealthMasterAsync(int productId, string author);

        int AddHealthMaster(int productId, string author);
        List<VCheckProductList> GetHealthProducts(string uId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">��Ʒid</param>
        /// <param name="uId">�û�id</param>
        /// <returns></returns>
        VCheckProductDetail GetHealthProductById(int id, string uId);
        /// <summary>
        /// ͨ��Id��ȡ����
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        HealthOrderMaster GetHealthMaster(int id);

        HealthOrderMaster GetHealthMaster(int id, string author);
        VHealthEntryInfo GetHealthEntryInfo(int matserId, string author);
        IPagedList<HealthOrderDetail> GetHealthOrderDetails(int pageIndex, int pageSize, int masterId);

        VHealthConfirmPayment GetConfirmPayment(int masterId);
        int UploadEmpExcel(HttpPostedFileBase empinfo, int masterId, string author);
        void UpdateMaster(HealthOrderMaster master);
    }
}