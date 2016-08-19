using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core.Pager;
using Domain;
using Models;
using Models.Order;
using System;
using Domain.Health;

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

        HealthOrderMaster AddHealthMaster(int productId, string author, int count);
        List<HealthOrderMaster> AddHealthMaster(List<Models.Cart.CartBuyModel> model, string author);
        List<HealthOrderMaster> GetByTicks(string ticks, string author = null);

        bool DeleteMaster(int masterID, string author);
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
        HealthOrderMaster GetHealthMaster(int id, string dateTicks = "", string author = "");
        VHealthEntryInfo GetHealthEntryInfo(int matserId, string author);
        VHealthAuditOrder GetHealthAuditOrder(int matserId, string dateTicks);
        IPagedList<HealthOrderDetail> GetHealthOrderDetails(int pageIndex, int pageSize, int masterId, long ticks);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="uName"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        IPagedList<VHealthAuditList> GetHealthAuditList(int pageIndex, int pageSize, string uName, VHealthSearch search);
        VHealthConfirmPayment GetConfirmPayment(int masterId, string dateTicks);
        SelectList GetListType(string uId);
        string GetPaymentNoticePdf(string dateTicks);
        Task GetPaymentNoticePdfAsync(string dateTicks);
        string UploadEmpExcel(HttpPostedFileBase empinfo, int masterId, string author);
        void UpdateMaster(HealthOrderMaster master);
        bool InsertHealthFile(HealthFile item);
        bool DeleteHealthEmp(int id);
        IPagedList<VHealthAuditList> GetHealthList(int status = 0, DateTime? beginDate = null, DateTime? endDate = null, string productName = null, string orderNumber = null, int pageIndex = 1, int pageSize = 15);
        IPagedList<HealthEmpTemp> GetHealthEmpTemp(int pageIndex, int pageSize, int masterId, long ticks);
        void PutEmp(int id);
    }
}