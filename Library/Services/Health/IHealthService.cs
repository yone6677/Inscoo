using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
        /// <summary>
        /// index ҳ��
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        List<VCheckProductList> GetHealthProducts(string uId);
        /// <summary>
        /// buyinfo ҳ��
        /// </summary>
        /// <param name="uId"></param>
        /// <param name="productType"></param>
        /// <param name="productName"></param>
        /// <returns></returns>
        List<VCheckProductList> GetHealthProducts(string uId, string productType, string productName);
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
        VHealthAuditOrder GetHealthAuditOrder(int matserId);
        IPagedList<HealthOrderDetail> GetHealthOrderDetails(int pageIndex, int pageSize, int masterId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="uName"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        IPagedList<VHealthAuditList> GetHealthAuditList(int pageIndex, int pageSize, string uName, VHealthSearch search);
        VHealthConfirmPayment GetConfirmPayment(int masterId);
        SelectList GetListType(string uId);
        string GetPaymentNoticePdf(int masterId);
        Task GetPaymentNoticePdfAsync(int masterId);
        int UploadEmpExcel(HttpPostedFileBase empinfo, int masterId, string author);
        void UpdateMaster(HealthOrderMaster master);
    }
}